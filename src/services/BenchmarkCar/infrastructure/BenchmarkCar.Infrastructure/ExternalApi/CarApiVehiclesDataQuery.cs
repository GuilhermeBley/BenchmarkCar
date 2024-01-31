using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.Application.IntegrationEvents.CreateModelsByMake;
using BenchmarkCar.Application.IntegrationEvents.MakesRequestedToCreate;
using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;
using BenchmarkCar.Infrastructure.Model.CarApi;
using BenchmarkCar.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading;

namespace BenchmarkCar.Infrastructure.ExternalApi;

internal class CarApiVehiclesDataQuery
    : IVehiclesDataQuery
{
    private const string URL = "https://carapi.app/";

    public readonly static Action<IServiceProvider, HttpClient> Configure =
        (provider, client) =>
        {
            client.BaseAddress = new Uri(URL);
            client.Timeout = new TimeSpan(0, 5, 0);
        };
    private readonly HttpClient _httpClient;
    private readonly IOptions<CarApiOptions> _options;

    /// <summary>
    /// Lock all the requests
    /// </summary>
    /// <remarks>
    ///     <para>It's needed to avoid cross-origin</para>
    /// </remarks>
    private static SemaphoreSlim _lockRequest
        = new(1, 1);

    private static SemaphoreSession _semaphoreSession
        => new(_lockRequest);

    private LoginSession _loginSession
        => new(_httpClient, _options);

    public CarApiVehiclesDataQuery(
        HttpClient httpClient,
            IOptions<CarApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<CreateVehicleModelApiDetails> GetByExternalModelId(
        object modelId,
        CancellationToken cancellationToken = default)
    {
        if (!(modelId is string modelTextId))
            throw new CommonCoreException("Invalid id type.");

        using var session = _semaphoreSession;

        await session.WaitAsync(cancellationToken);

        await _loginSession.EnsureClientLoggedAsync(cancellationToken);

        const string PATH = "api/trims/{id}";

        using var response
            = await _httpClient.GetAsync(
                PATH.Replace("{id}", modelTextId),
                cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to get data. Staus: {response.StatusCode}, Body: {body}");
        }

        var apiModel = await response.Content.ReadFromJsonAsync<VehicleTrimResponse>();

        var createBodyModel = apiModel?.MakeModelTrimBody?.MapToCreateBodyModel();

        ArgumentNullException.ThrowIfNull(createBodyModel, nameof(createBodyModel));

        var createEngineModel = apiModel?.MakeModelTrimEngine?.MapToCreateEngineModel();

        ArgumentNullException.ThrowIfNull(createEngineModel, nameof(createEngineModel));

        return new CreateVehicleModelApiDetails(
            createBodyModel,
            createEngineModel);
    }

    public async IAsyncEnumerable<CreateMakeModel> GetAllMakesAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var session = _semaphoreSession;

        await session.WaitAsync(cancellationToken);

        await _loginSession.EnsureClientLoggedAsync(cancellationToken);

        const string PATH = "api/makes?page={page}";

        for (int page = 1; ; page++)
        {

            using var response
                = await _httpClient.GetAsync(
                    PATH.Replace("{page}", page.ToString()),
                    cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                yield break;

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<VehicleMakeResponse>();

            if (result is null ||
                result.Data is null)
                yield break;

            foreach (var apiMake in result.Data)
                yield return new CreateMakeModel(
                    apiMake.Id.ToString(),
                    apiMake.Name ?? string.Empty);
        }
    }

    public async IAsyncEnumerable<CreateApiModelSummaryModel> GetByModelsSummaryByMakeAsync(
        object makeId,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var session = _semaphoreSession;

        await session.WaitAsync(cancellationToken);

        await _loginSession.EnsureClientLoggedAsync(cancellationToken);

        const string PATH = "api/trims?make_id={makeId}&page={page}";

        for (int page = 1; ; page++)
        {

            using var response
                = await _httpClient.GetAsync(
                    PATH
                        .Replace("{makeId}", makeId.ToString())
                        .Replace("{page}", page.ToString()),
                    cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                yield break;

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<VehicleTrimSummaryResponse>();

            if (result is null ||
                result.Data is null)
                yield break;

            foreach (var apiSummaryModel in result.Data)
                yield return new CreateApiModelSummaryModel(
                    ExternalId: apiSummaryModel.Id ?? 
                        throw new CommonCoreException("Invalid id."),
                    Year: apiSummaryModel.Year ?? 
                        throw new CommonCoreException("Invalid year."),
                    Name: apiSummaryModel.Name ?? string.Empty,
                    Description: apiSummaryModel.Description ?? string.Empty);
        }
    }

    /// <summary>
    /// Manages login from current client
    /// </summary>
    private class LoginSession
    {
        private string? _lastToken;
        private HttpClient _httpClient;
        private readonly IOptions<CarApiOptions> _options;

        public LoginSession(
            HttpClient httpClient,
            IOptions<CarApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task EnsureClientLoggedAsync(CancellationToken cancellationToken = default)
        {
            var token = await GetTokenAsync(cancellationToken);

            _httpClient.DefaultRequestHeaders.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }

        private async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            const string PATH = "api/auth/login";

            if (_lastToken is not null &&
                IsntExpired(_lastToken))
                return _lastToken;

            using var result = await _httpClient.PostAsJsonAsync(
                PATH,
                new
                {
                    api_token = _options.Value.ApiToken,
                    api_secret = _options.Value.ApiSecret
                },
                cancellationToken);

            result.EnsureSuccessStatusCode();

            _lastToken = await result.Content.ReadAsStringAsync();
            return _lastToken;
        }

        private static bool IsntExpired(string token)
            => !IsExpired(token);

        private static bool IsExpired(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jsonToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null || jsonToken.ValidTo < DateTime.UtcNow.AddHours(-1))
            {
                return true; // Token is expired
            }

            return false;
        }
    }

    private class SemaphoreSession
        : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;

        public SemaphoreSession(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public Task WaitAsync(CancellationToken cancellationToken = default)
            => _semaphore.WaitAsync(cancellationToken);

        public void Dispose()
            => _semaphore.Release();
    }
}

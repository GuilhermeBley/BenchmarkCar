using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;
using BenchmarkCar.Infrastructure.Model.CarApi;
using BenchmarkCar.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;

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
            = await _httpClient.GetAsync(PATH, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to get data. Staus: {response.StatusCode}, Body: {body}");
        }

        var internalModelBody = await response.Content.ReadFromJsonAsync<VehicleTrimResponse>();

        return new CreateVehicleModelApiDetails()
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
            const string PATH = "/auth/login";

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

            return await result.Content.ReadAsStringAsync();
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

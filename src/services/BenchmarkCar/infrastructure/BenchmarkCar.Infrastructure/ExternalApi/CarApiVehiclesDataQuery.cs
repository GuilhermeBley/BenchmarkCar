using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;

namespace BenchmarkCar.Infrastructure.ExternalApi;

internal class CarApiVehiclesDataQuery
    : IVehiclesDataQuery
{
    private const string URL = "https://carapi.app/";

    public Task<CreateVehicleModelApiDetails> GetByExternalModelId(
        object modelId, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

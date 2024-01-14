using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;

namespace BenchmarkCar.Application.ExternalApi;

public interface IVehiclesDataQuery
{
    Task<CreateVehicleModelApiDetails> GetByExternalModelId(
        object modelId, 
        CancellationToken cancellationToken = default);
}

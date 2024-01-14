using BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

namespace BenchmarkCar.Application.ExternalApi;

public interface IVehiclesDataQuery
{
    Task<CreateVehicleModelDetailsRequest> GetByExternalModelId(
        object modelId, 
        CancellationToken cancellationToken = default);
}

using BenchmarkCar.Application.Commands.CreateVehicleModelDetails;
using BenchmarkCar.Application.IntegrationEvents.CreateModelsByMake;
using BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;
using BenchmarkCar.Application.IntegrationEvents.MakesRequestedToCreate;

namespace BenchmarkCar.Application.ExternalApi;

public interface IVehiclesDataQuery
{
    Task<CreateVehicleComparativeVehicleDataModel> GetByExternalModelId(
        object modelId, 
        CancellationToken cancellationToken = default);
    IAsyncEnumerable<CreateMakeModel> GetAllMakesAsync(
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<CreateApiModelSummaryModel> GetByModelsSummaryByMakeAsync(
        object makeId,
        CancellationToken cancellationToken = default);
}

using BenchmarkCar.Application.IntegrationEvents.CreateModelsByMake;
using BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;
using BenchmarkCar.Application.IntegrationEvents.MakesRequestedToCreate;
using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;

namespace BenchmarkCar.Application.ExternalApi;

public interface IVehiclesDataQuery
{
    Task<CreateVehicleModelApiDetails> GetByExternalModelId(
        object modelId, 
        CancellationToken cancellationToken = default);
    IAsyncEnumerable<CreateMakeModel> GetAllMakesAsync(
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<CreateApiModelSummaryModel> GetByModelsSummaryByMakeAsync(
        object makeId,
        CancellationToken cancellationToken = default);

    Task<CreateVehicleComparativeVehicleDataModel> GetDataByModelIdAsync(
        object modelId,
        CancellationToken cancellationToken = default);
}

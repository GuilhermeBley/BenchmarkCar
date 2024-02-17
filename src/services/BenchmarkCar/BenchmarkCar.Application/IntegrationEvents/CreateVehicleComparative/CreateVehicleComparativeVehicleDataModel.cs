using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearch;

namespace BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;

public record CreateVehicleComparativeVehicleDataModel(
    CreateEngineModel Engine,
    CreateBodyModel Body);

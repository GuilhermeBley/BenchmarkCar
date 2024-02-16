using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;

namespace BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;

public record CreateVehicleComparativeVehicleDataModel(
    CreateEngineModel Engine,
    CreateBodyModel Body);

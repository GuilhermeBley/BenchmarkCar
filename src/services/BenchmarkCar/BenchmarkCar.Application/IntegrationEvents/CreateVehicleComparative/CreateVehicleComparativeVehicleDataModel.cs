using BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

namespace BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;

public record CreateVehicleComparativeVehicleDataModel(
    CreateEngineModel Engine,
    CreateBodyModel Body);

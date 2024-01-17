using BenchmarkCar.Application.Model.Vehicles;
using MediatR;

namespace BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;

public record CreateVehicleModelApiDetails(
    CreateBodyModel? Body,
    CreateEngineModel? Engine);

using BenchmarkCar.Application.Model.Vehicles;
using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

public record CreateVehicleModelDetailsRequest(
    VehicleModelModel Vehicle,
    CreateBodyModel? Body,
    CreateEngineModel? Engine)
    : IRequest<CreateVehicleModelDetailsResponse>;

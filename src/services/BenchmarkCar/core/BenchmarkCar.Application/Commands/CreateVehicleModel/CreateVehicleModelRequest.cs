using BenchmarkCar.Application.Model.Vehicles;
using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleModel;

public record CreateVehicleModelRequest(
    VehicleMakeModel VehicleMake,
    string Name,
    string ExternalId,
    int Year,
    string? Description = null)
    : IRequest<CreateVehicleModelResponse>;

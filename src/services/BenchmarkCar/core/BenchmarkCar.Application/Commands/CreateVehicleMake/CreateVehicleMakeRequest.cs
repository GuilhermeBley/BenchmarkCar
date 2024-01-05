using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleMake;

public record CreateVehicleMakeRequest
    (string Name,
    string ExternalId)
    : IRequest<CreateVehicleMakeResponse>;

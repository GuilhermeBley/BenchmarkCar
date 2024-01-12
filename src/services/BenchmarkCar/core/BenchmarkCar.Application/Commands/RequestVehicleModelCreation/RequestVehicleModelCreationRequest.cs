using MediatR;

namespace BenchmarkCar.Application.Commands.RequestVehicleModelCreation;

public record RequestVehicleModelCreationRequest(
    Guid MakeId)
    : IRequest<RequestVehicleModelCreationResponse>;

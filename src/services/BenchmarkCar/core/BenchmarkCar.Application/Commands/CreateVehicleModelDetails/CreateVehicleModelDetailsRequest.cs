using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

public record CreateVehicleModelDetailsRequest
    : IRequest<CreateVehicleModelDetailsResponse>;

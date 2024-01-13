using MediatR;

namespace BenchmarkCar.Application.Commands.GetVehicleCompleteData;

public record GetVehicleCompleteDataRequest(
    Guid modelId)
    : IRequest<GetVehicleCompleteDataResponse>;

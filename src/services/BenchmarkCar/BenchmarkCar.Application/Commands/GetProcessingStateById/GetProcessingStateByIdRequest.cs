using MediatR;

namespace BenchmarkCar.Application.Commands.GetProcessingStateById;

public record GetProcessingStateByIdRequest(
    Guid ProcessId)
    : IRequest<GetProcessingStateByIdResponse>;

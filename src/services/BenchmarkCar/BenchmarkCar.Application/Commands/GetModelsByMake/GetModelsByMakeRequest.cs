using MediatR;

namespace BenchmarkCar.Application.Commands.GetModelsByMake;

public record GetModelsByMakeRequest(
    Guid MakeId)
    : IRequest<GetModelsByMakeResponse>;

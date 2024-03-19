using MediatR;

namespace BenchmarkCar.Application.Commands.GetModelsByName;

public record GetMakesAndModelsByFilterRequest(
    string? Filter)
    : IRequest<GetMakesAndModelsByFilterResponse>;
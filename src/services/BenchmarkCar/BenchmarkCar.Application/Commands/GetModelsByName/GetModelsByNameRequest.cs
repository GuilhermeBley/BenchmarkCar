using MediatR;

namespace BenchmarkCar.Application.Commands.GetModelsByName;


public record GetModelsByNameRequest(
    string? Filter)
    : IRequest<GetModelsByNameResponse>;
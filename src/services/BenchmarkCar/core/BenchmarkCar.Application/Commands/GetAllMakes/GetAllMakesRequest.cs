using MediatR;

namespace BenchmarkCar.Application.Commands.GetAllMakes;

internal record GetAllMakesRequest()
    : IRequest<GetAllMakesResponse>;

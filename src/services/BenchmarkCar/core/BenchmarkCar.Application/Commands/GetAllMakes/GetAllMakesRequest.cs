using MediatR;

namespace BenchmarkCar.Application.Commands.GetAllMakes;

public record GetAllMakesRequest()
    : IRequest<IEnumerable<MakeResponse>>;

public record MakeResponse(
    Guid Id,
    string NormalizedName,
    string Name);

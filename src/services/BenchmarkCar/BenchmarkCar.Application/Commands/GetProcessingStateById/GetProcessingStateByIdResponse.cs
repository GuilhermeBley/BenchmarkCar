using BenchmarkCar.Application.Model.Queue;
using BenchmarkCar.Domain.Entities.Queue;

namespace BenchmarkCar.Application.Commands.GetProcessingStateById;

public record GetProcessingStateByIdResponse(
    Guid Id,
    int Code,
    double Percent,
    object? Result = null);

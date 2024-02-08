using MediatR;

namespace BenchmarkCar.Application.Commands.RequestVehicleModelComparative;

public record RequestVehicleModelComparativeRequest(
    Guid modelIdX,
    Guid modelIdY)
    : IRequest<RequestVehicleModelComparativeResponse>;

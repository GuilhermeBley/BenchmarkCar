using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleMakeSummary;

public record RequestVehicleModelSummaryRequest(
    Guid MakeId)
    : IRequest<RequestVehicleModelSummaryResponse>;

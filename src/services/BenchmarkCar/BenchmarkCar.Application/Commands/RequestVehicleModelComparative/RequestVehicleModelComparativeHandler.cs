using MediatR;

namespace BenchmarkCar.Application.Commands.RequestVehicleModelComparative;

public class RequestVehicleModelComparativeHandler
    : IRequestHandler<RequestVehicleModelComparativeRequest, RequestVehicleModelComparativeResponse>
{
    public Task<RequestVehicleModelComparativeResponse> Handle(
        RequestVehicleModelComparativeRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleMake;

public class CreateVehicleMakeHandler
    : IRequestHandler<CreateVehicleMakeRequest, CreateVehicleMakeResponse>
{
    public Task<CreateVehicleMakeResponse> Handle(
        CreateVehicleMakeRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

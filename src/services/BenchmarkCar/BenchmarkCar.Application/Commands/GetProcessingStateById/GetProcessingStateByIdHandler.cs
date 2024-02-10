using MediatR;

namespace BenchmarkCar.Application.Commands.GetProcessingStateById;

public class GetProcessingStateByIdHandler
    : IRequestHandler<GetProcessingStateByIdRequest, GetProcessingStateByIdResponse>
{
    public Task<GetProcessingStateByIdResponse> Handle(
        GetProcessingStateByIdRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

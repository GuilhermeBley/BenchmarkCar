using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

/// <summary>
/// Add a new vehicle model details, the data is about model and engine.
/// </summary>
/// <remarks>
///     <para>Try add model to best models.</para>
/// </remarks>
public class CreateVehicleModelDetailsHandler
    : IRequestHandler<CreateVehicleModelDetailsRequest, CreateVehicleModelDetailsResponse>
{
    public Task<CreateVehicleModelDetailsResponse> Handle(
        CreateVehicleModelDetailsRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BenchmarkCar.Ui.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleMakeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VehicleModelController> _logger;

    public VehicleMakeController(
        IMediator mediator,
        ILogger<VehicleModelController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("Request")]
    public async Task<ActionResult> RequestToCreateOrUpdateMakesAsync(
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Requesting make creation at {0}.", DateTimeOffset.UtcNow);

        var result = await _mediator.Send(
            new Application.Commands.RequestVehicleMakeCreation.RequestVehicleMakeCreationRequest(),
            cancellationToken);

        return Ok(result);
    }
}
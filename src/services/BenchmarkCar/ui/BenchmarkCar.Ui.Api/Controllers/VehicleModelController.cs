using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BenchmarkCar.Ui.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleModelController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VehicleModelController> _logger;

    public VehicleModelController(
        IMediator mediator, 
        ILogger<VehicleModelController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("Vehicle-Data/{id}")]
    public async Task<ActionResult> GetVehicleCompleteDataAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Requesting vehicle complete data '{0}' at {1}.", 
            id, DateTimeOffset.UtcNow);

        var result = await _mediator.Send(
            new Application.Commands.GetVehicleCompleteData.GetVehicleCompleteDataRequest(id),
            cancellationToken);

        return Ok(result);
    }
}

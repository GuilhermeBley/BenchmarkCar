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

    [HttpPost("request/make/{makeId}")]
    public async Task<ActionResult> RequestToCreateOrUpdateModelsAsync(
        Guid makeId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Requesting make creation at {0}.", DateTimeOffset.UtcNow);

        var result = await _mediator.Send(
            new BenchmarkCar.Application.Commands.CreateVehicleMakeSummary.RequestVehicleModelSummaryRequest(makeId),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("make/{makeId}")]
    public async Task<ActionResult> GetVehiclesModelsByMakeIdAsync(
        Guid makeId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Requesting vehicle complete data '{0}' at {1}.",
            makeId, DateTimeOffset.UtcNow);

        var result = await _mediator.Send(
            new Application.Commands.GetModelsByMake.GetModelsByMakeRequest(makeId),
            cancellationToken);

        return Ok(result.Values);
    }
}

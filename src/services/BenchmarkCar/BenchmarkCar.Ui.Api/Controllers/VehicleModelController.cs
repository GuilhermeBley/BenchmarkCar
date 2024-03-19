using BenchmarkCar.Ui.Api.Model;
using MediatR;
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

    [HttpPost("model/request/comparative")]
    public async Task<ActionResult> RequestToCompareModelsAsync(
        RequestComparativeModelViewModel model,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Requesting model comparative at {0}.", DateTimeOffset.UtcNow);

        var result = await _mediator.Send(
            new BenchmarkCar.Application.Commands.RequestVehicleModelComparative.RequestVehicleModelComparativeRequest(
                model.ModelIdX,
                model.ModelIdY),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("model/request/comparative/{id}")]
    public async Task<ActionResult> GetCompareModelByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Get model comparative at {0}.", DateTimeOffset.UtcNow);

        var result = await _mediator.Send(
            new BenchmarkCar.Application.Commands.GetProcessingStateById.GetProcessingStateByIdRequest(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("make/request/{makeId}")]
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

    [HttpGet("make-model")]
    public async Task<ActionResult> GetMakeAndModelByFilterAsycn(
        [FromQuery] string? filter,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Requesting make and model '{0}' at {1}.",
            filter, DateTimeOffset.UtcNow);

        var result = await _mediator.Send(
            new BenchmarkCar.Application.Commands.GetModelsByName.GetMakesAndModelsByFilterRequest(filter),
            cancellationToken);

        return Ok(result.Items);
    }
}

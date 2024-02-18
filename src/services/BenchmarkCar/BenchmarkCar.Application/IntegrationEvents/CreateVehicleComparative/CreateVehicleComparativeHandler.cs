using BenchmarkCar.Application.Commands.CreateVehicleModelDetails;
using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.Application.Model.Queue;
using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;

public class CreateVehicleComparativeHandler
    : IIntegrationEventHandler<EventBus.Events.RequestComparativeModelIntegrationEvent>
{
    private static readonly SemaphoreSlim _lockVehicleCollectModelInfo
        = new SemaphoreSlim(1, 1);

    private readonly BenchmarkVehicleContext _vehicleContext;
    private readonly IVehiclesDataQuery _api;
    private readonly IMediator _mediator;
    private readonly ICoreLogger<CreateVehicleComparativeHandler> _logger;

    public CreateVehicleComparativeHandler(
        BenchmarkVehicleContext vehicleContext, 
        IVehiclesDataQuery api, 
        IMediator mediator, 
        ICoreLogger<CreateVehicleComparativeHandler> logger)
    {
        _vehicleContext = vehicleContext;
        _api = api;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(
        RequestComparativeModelIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var bestModel =
            await _vehicleContext
            .BestModels
            .AsNoTracking()
            .FirstOrDefaultAsync();

        var vehicleX =
            await _vehicleContext
            .VehiclesModels
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == @event.ModelIdX);

        if (vehicleX is null)
        {
            _logger.LogInformation("Vehicle '{0}' was not found.", @event.ModelIdX);
            await MarkProccessAsErrorAsync(@event.ProccessId);
            return;
        }

        var vehicleY =
            await _vehicleContext
            .VehiclesModels
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == @event.ModelIdY);


        if (vehicleY is null)
        {
            _logger.LogInformation("Vehicle '{0}' was not found.", @event.ModelIdY);
            await MarkProccessAsErrorAsync(@event.ProccessId);
            return;
        }

        var vehicleDataResultX = await GetCachedOrCollectVehicleDataAsync(
            modelId: vehicleX.Id,
            externalModelId: vehicleX.ExternalId,
            cancellationToken: cancellationToken);

        var vehicleDataResultY = await GetCachedOrCollectVehicleDataAsync(
            modelId: vehicleY.Id, 
            externalModelId: vehicleY.Id,
            cancellationToken: cancellationToken);

        await MarkProccessAsCompletedAsync(
            @event.ProccessId,
            new { bestModel, vehicleDataResultX, vehicleDataResultY },
            cancellationToken);

        _logger.LogInformation(
            "Vehicles '{0}' and '{1} are successfully collecteds.", 
            @event.ModelIdX,
            @event.ModelIdY);
    }

    private async Task MarkProccessAsErrorAsync(
        Guid proccessId,
        CancellationToken cancellationToken = default)
    {
        var processingQueueStateModel =
            (await _vehicleContext
                .ProcessingQueues
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == proccessId, cancellationToken));

        if (processingQueueStateModel is null)
            return;

        var processingQueueState = processingQueueStateModel.MapToEntity();

        try
        {
            processingQueueState.FinishWithStatusCode(Domain.Entities.Queue.ProcessingStateCode.StopedWitError);
        }
        catch { }

        processingQueueStateModel.Code = (int)processingQueueState.Code;

        _vehicleContext.ProcessingQueues.Update(processingQueueStateModel);

        await _vehicleContext.SaveChangesAsync();
    }

    private async Task MarkProccessAsCompletedAsync<T>(
        Guid proccessId,
        T result,
        CancellationToken cancellationToken = default)
    {
        var processingQueueStateModel =
            (await _vehicleContext
                .ProcessingQueues
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == proccessId, cancellationToken));

        if (processingQueueStateModel is null)
            return;

        var processingQueueState = processingQueueStateModel.MapToEntity();

        try
        {
            processingQueueState.FinishWithStatusCode(Domain.Entities.Queue.ProcessingStateCode.Processed);
            processingQueueState.ChangePercentTo(100);
            processingQueueState.TrySetResult(result);
        }
        catch { }

        processingQueueStateModel.Code = (int)processingQueueState.Code;

        using var transaction =
            await _vehicleContext.Database.BeginTransactionAsync(cancellationToken);

        _vehicleContext.ProcessingQueues.Update(processingQueueStateModel);
        if (processingQueueState.Result is not null)
            await _vehicleContext.ProcessingResults.AddAsync(
                ProcessingResultModel.MapFrom(processingQueueState.Result));

        await transaction.CommitAsync(cancellationToken);

        await _vehicleContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<VehicleDataResult> GetCachedOrCollectVehicleDataAsync(
        Guid modelId,
        object externalModelId,
        CancellationToken cancellationToken = default)
    {
        var result = await TryGetCachedAsync(modelId, cancellationToken);

        if (result is not null)
            return result;

        try
        {
            await _lockVehicleCollectModelInfo.WaitAsync(cancellationToken);

            // Checking again
            result = await TryGetCachedAsync(modelId, cancellationToken);

            if (result is not null)
                return result;

            var apiResult = await _api.GetByExternalModelId(externalModelId, cancellationToken);

            // send to mediatr cache info on data base
            await _mediator.Send(new CreateVehicleModelDetailsRequest
            (
                ModelId: modelId,
                Engine: apiResult.Engine,
                Body: apiResult.Body
            ), cancellationToken);
        }
        finally
        {
            _lockVehicleCollectModelInfo.Release();
        }

        return await TryGetCachedAsync(modelId, cancellationToken)
            ?? throw new CommonCoreException("Failed to get data.");
    }

    private async Task<VehicleDataResult?> TryGetCachedAsync(
        Guid modelId,
        CancellationToken cancellationToken = default)
    {

        var body =
            await _vehicleContext
                .ModelBodies
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ModelId == modelId, cancellationToken);

        var engine =
            await _vehicleContext
                .EngineModels
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ModelId == modelId, cancellationToken);

        if (engine is not null &&
            body is not null)
            return new(engine, body);

        return null;
    }

    private record VehicleDataResult(
        ModelEngineModel Engine,
        ModelBodyModel Body);
}

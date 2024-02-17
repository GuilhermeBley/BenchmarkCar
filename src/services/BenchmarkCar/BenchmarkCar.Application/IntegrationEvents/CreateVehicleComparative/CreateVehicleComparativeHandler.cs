using BenchmarkCar.Application.ExternalApi;
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
    // add logs

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
            await MarkProccessAsErrorAsync(@event.ProccessId);
            return;
        }

        throw new NotImplementedException();
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

            // send to mediatr CreateVehicleModelDetailsRequest

            throw new NotImplementedException();
        }
        finally
        {
            _lockVehicleCollectModelInfo.Release();
        }
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

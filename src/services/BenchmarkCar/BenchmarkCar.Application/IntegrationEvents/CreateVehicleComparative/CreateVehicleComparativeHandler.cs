using BenchmarkCar.Application.Repositories;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;

public class CreateVehicleComparativeHandler
    : IIntegrationEventHandler<EventBus.Events.RequestComparativeModelIntegrationEvent>
{
    private readonly BenchmarkVehicleContext _vehicleContext;
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
}

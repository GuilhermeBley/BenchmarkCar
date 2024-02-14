using BenchmarkCar.Application.Repositories;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;

public class CreateVehicleComparativeHandler
    : IIntegrationEventHandler<EventBus.Events.RequestComparativeModelIntegrationEvent>
{
    private readonly BenchmarkVehicleContext _vehicleContext;

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

    private Task MarkProccessAsErrorAsync(Guid proccessId)
    {
        return Task.CompletedTask;
    }
}

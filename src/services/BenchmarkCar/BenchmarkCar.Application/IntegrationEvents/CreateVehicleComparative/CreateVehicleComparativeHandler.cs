using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;

namespace BenchmarkCar.Application.IntegrationEvents.CreateVehicleComparative;

public class CreateVehicleComparativeHandler
    : IIntegrationEventHandler<EventBus.Events.RequestComparativeModelIntegrationEvent>
{
    public Task Handle(
        RequestComparativeModelIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

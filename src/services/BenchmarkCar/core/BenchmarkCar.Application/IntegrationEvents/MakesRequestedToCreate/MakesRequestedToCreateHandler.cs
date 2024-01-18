using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;

namespace BenchmarkCar.Application.IntegrationEvents.MakesRequestedToCreate;

public class MakesRequestedToCreateHandler
    : IIntegrationEventHandler<CreateMakesIntegrationEvent>
{
    public Task Handle(
        CreateMakesIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        
    }
}

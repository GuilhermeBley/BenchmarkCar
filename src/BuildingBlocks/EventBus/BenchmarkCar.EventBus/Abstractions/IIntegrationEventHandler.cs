namespace BenchmarkCar.EventBus.Abstractions;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}

public interface IIntegrationEventHandler
{
}

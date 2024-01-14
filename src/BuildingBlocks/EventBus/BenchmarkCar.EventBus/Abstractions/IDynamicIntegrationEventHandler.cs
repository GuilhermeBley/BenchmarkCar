namespace BenchmarkCar.EventBus.Abstractions;

public interface IDynamicIntegrationEventHandler
{
    Task Handle(dynamic eventData, CancellationToken cancellationToken = default);
}

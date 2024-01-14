namespace BenchmarkCar.EventBus.Events;

public class CreateModelIntegrationEvent
    : IntegrationEvent
{
    public Guid ModelId { get; set; }
}

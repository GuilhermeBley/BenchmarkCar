namespace BenchmarkCar.EventBus.Events;

public class CreateModelIntegrationEvent
    : IntegrationEvent
{
    public Guid MakeId { get; set; }
}

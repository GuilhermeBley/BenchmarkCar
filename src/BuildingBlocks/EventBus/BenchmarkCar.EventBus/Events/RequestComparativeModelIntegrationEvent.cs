namespace BenchmarkCar.EventBus.Events;

public class RequestComparativeModelIntegrationEvent
    : IntegrationEvent
{
    public Guid ProccessId { get; set; }
    public Guid ModelIdX { get; set; }
    public Guid ModelIdY { get; set; }
}

namespace BenchmarkCar.EventBus.Events;

public class CreateModelsByMakeIntegrationEvent
    : IntegrationEvent
{
    public string MakeId { get; set; } = string.Empty;
    public string ExternalMakeId { get; set; } = string.Empty;
}

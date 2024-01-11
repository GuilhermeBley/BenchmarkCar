namespace BenchmarkCar.EventBus;

public class IntegrationEvent
{
    public IntegrationEvent()
    {
        EventId = Guid.NewGuid();
        EventCreationDate = DateTime.UtcNow;
    }

    public IntegrationEvent(Guid id, DateTime createDate)
    {
        EventId = id;
        EventCreationDate = createDate;
    }

    public Guid EventId { get; private set; }

    public DateTime EventCreationDate { get; private set; }
}
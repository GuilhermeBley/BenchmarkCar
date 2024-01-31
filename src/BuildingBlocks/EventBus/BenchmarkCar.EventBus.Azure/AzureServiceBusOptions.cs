namespace BenchmarkCar.EventBus.Azure;

public class AzureServiceBusOptions
{
    public const string SECTION = "AzureServiceBus";

    public string ConnectionString { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
    public string BenchmarkCarSub { get; set; } = string.Empty;
}

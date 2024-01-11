using Azure.Messaging.ServiceBus;

namespace BenchmarkCar.EventBus.Azure
{
    public interface IServiceBusPersisterConnection : IDisposable, IAsyncDisposable
    {
        ServiceBusSender CreateModel();
        ServiceBusProcessor SubscriptionProcessor { get; }
        ServiceBusRuleManager SubscriptionRuleManager { get; }
    }
}

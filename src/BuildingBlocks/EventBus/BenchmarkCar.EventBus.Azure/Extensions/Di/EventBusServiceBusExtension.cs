using BenchmarkCar.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BenchmarkCar.EventBus.Azure.Extensions.Di;

public static class EventBusServiceBusExtension
{
    public static IServiceCollection AddEventBus(
        this IServiceCollection serviceCollection,
        string subscriptionName,
        Action<IServiceProvider, IEventBus>? onConfigure = null)
    {
        serviceCollection.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        serviceCollection.AddSingleton<IEventBus>(
            provider => {

                var eventBusOptions = provider.GetRequiredService<IOptions<AzureServiceBusOptions>>();
                var logFactory = provider.GetRequiredService<ILoggerFactory>();

                var serviceBusPersisterConnection
                    = new DefaultServiceBusPersisterConnection(
                        logger: logFactory.CreateLogger<DefaultServiceBusPersisterConnection>(),
                        options: eventBusOptions,
                        subscription: subscriptionName
                    );

                var subsManager = provider.GetRequiredService<IEventBusSubscriptionsManager>();

                var eventBus = new EventBusServiceBus(
                    serviceBusPersisterConnection: serviceBusPersisterConnection,
                    logger: logFactory.CreateLogger<EventBusServiceBus>(),
                    subsManager: subsManager,
                    provider: provider.CreateScope().ServiceProvider);

                onConfigure?.Invoke(
                    provider.CreateScope().ServiceProvider,
                    eventBus
                );

                return eventBus;
            });

        return serviceCollection;
    }
}

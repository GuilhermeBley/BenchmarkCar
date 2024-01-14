using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using BenchmarkCar.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace BenchmarkCar.EventBus.Azure;

internal class EventBusServiceBus : IEventBus
{
    private const string INTEGRATION_EVENT_SUFIX = nameof(IntegrationEvent);

    private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
    private readonly ILogger<EventBusServiceBus> _logger;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly IServiceScope _scopeLifeTime;

    private ServiceBusProcessor _subscriptionProcessor 
        => _serviceBusPersisterConnection.SubscriptionProcessor;
    private ServiceBusRuleManager _subscriptionRuleManager 
        => _serviceBusPersisterConnection.SubscriptionRuleManager;

    public EventBusServiceBus(
        IServiceBusPersisterConnection serviceBusPersisterConnection,
        ILogger<EventBusServiceBus> logger, 
        IEventBusSubscriptionsManager subsManager, 
        IServiceProvider provider)
    {
        _serviceBusPersisterConnection = serviceBusPersisterConnection;
        _logger = logger;
        _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
        _scopeLifeTime = provider.CreateScope();
        RegisterSubscriptionClientMessageHandler();
    }

    public async Task PublishAsync(IntegrationEvent @event)
    {
        var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFIX, string.Empty);
        var jsonMessage = JsonSerializer.Serialize(@event, @event.GetType());
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var message = new ServiceBusMessage(body)
        {
            MessageId = Guid.NewGuid().ToString(),
            Subject = eventName
        };

        var topicClient = _serviceBusPersisterConnection.CreateModel();

        await topicClient.SendMessageAsync(message);
    }

    public void SubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler
    {
        _subsManager.AddDynamicSubscription<TH>(eventName);
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");

        var containsKey = _subsManager.HasSubscriptionsForEvent<T>();
        if (!containsKey)
        {
            try
            {
                _subscriptionRuleManager
                    .CreateRuleAsync(
                    eventName,
                    new CorrelationRuleFilter
                    {
                         Subject = eventName,
                    }).GetAwaiter().GetResult();
            }
            catch (ServiceBusException)
            {
                _logger.LogInformation($"The messaging entity {eventName} already exists.");
            }
        }

        _subsManager.AddSubscription<T, TH>();
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");

        try
        {
            _subscriptionRuleManager
             .DeleteRuleAsync(eventName)
             .GetAwaiter()
             .GetResult();
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            _logger.LogInformation($"The messaging entity {eventName} Could not be found.");
        }

        _subsManager.RemoveSubscription<T, TH>();
    }

    public void UnsubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler
    {
        _subsManager.RemoveDynamicSubscription<TH>(eventName);
    }

    public void Dispose()
    {
        _subsManager.Clear();
        _serviceBusPersisterConnection.Dispose();
    }

    private void RegisterSubscriptionClientMessageHandler()
    {
        _subscriptionProcessor.ProcessMessageAsync += async (psm) =>
            {
                var eventName = $"{psm.Message.Subject}";
                var messageData = Encoding.UTF8.GetString(psm.Message.Body);
                
                // Complete the message so that it is not received again.
                if (await ProcessEvent(eventName, messageData))
                {
                    await psm.CompleteMessageAsync(psm.Message);
                }
            };
        _subscriptionProcessor.ProcessErrorAsync += ExceptionReceivedHandler;
        
        _subscriptionProcessor.StartProcessingAsync().GetAwaiter().GetResult();
    }

    private Task ExceptionReceivedHandler(ProcessErrorEventArgs exceptionReceivedEventArgs)
    {
        Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
        Console.WriteLine("Exception context for troubleshooting:");
        Console.WriteLine($"- Entity Path: {exceptionReceivedEventArgs.EntityPath}");
        return Task.CompletedTask;
    }

    private async Task<bool> ProcessEvent(string eventName, string message, CancellationToken cancellationToken = default)
    {
        var processed = false;
        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            await using (var scope = _scopeLifeTime.ServiceProvider.CreateAsyncScope())
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        var handler = ActivatorUtilities.CreateInstance(scope.ServiceProvider, subscription.HandlerType) as IDynamicIntegrationEventHandler;
                        if (handler == null) continue;
                        dynamic? eventData = JsonSerializer.Deserialize<dynamic>(message);
                        await handler.Handle(eventData, cancellationToken);
                    }
                    else
                    {
                        var handler = ActivatorUtilities.CreateInstance(scope.ServiceProvider, subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        if (eventType is null) continue;
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                        if (integrationEvent is null) continue;
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        var tsk = concreteType?.GetMethod(nameof(IIntegrationEventHandler<IntegrationEvent>.Handle))?
                            .Invoke(handler, new object[] { integrationEvent, cancellationToken }) as Task;
                        if (tsk is not null)
                            await tsk.ConfigureAwait(false);
                    }
                }
            }
            processed = true;
        }
        return processed;
    }
}

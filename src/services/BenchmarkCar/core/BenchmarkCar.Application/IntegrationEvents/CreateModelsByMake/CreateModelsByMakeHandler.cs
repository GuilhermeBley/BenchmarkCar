using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;

namespace BenchmarkCar.Application.IntegrationEvents.CreateModelsByMake;

public class CreateModelsByMakeHandler
    : IIntegrationEventHandler<CreateModelsByMakeIntegrationEvent>
{
    private readonly IVehiclesDataQuery _api;
    private readonly ICoreLogger<CreateModelsByMakeHandler> _logger;

    public CreateModelsByMakeHandler(
        IVehiclesDataQuery api,
        ICoreLogger<CreateModelsByMakeHandler> logger)
    {
        _api = api;
        _logger = logger;
    }

    public Task Handle(
        CreateModelsByMakeIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

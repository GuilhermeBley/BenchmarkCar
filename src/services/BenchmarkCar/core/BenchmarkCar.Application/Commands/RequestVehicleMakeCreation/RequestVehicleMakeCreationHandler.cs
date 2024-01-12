using BenchmarkCar.Application.Repositories;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.RequestVehicleMakeCreation;

/// <summary>
/// Request to create or update all vehicles makes
/// </summary>
public class RequestVehicleMakeCreationHandler
    : IRequestHandler<RequestVehicleMakeCreationRequest, RequestVehicleMakeCreationResponse>
{
    private readonly IEventBus _eventBus;
    private readonly VehicleContext _vehicleContext;

    public RequestVehicleMakeCreationHandler(
        IEventBus eventBus,
        VehicleContext vehicleContext)
    {
        _eventBus = eventBus;
        _vehicleContext = vehicleContext;
    }

    public async Task<RequestVehicleMakeCreationResponse> Handle(
        RequestVehicleMakeCreationRequest request, 
        CancellationToken cancellationToken)
    {
        var @event =
            new CreateMakesIntegrationEvent();

        await _eventBus.PublishAsync(@event);

        var vehicleCount =
            await _vehicleContext.VehiclesMakes.CountAsync();

        return new RequestVehicleMakeCreationResponse(vehicleCount);
    }
}

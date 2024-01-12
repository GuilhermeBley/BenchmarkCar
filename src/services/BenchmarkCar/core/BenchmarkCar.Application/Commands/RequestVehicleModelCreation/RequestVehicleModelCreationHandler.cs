using BenchmarkCar.Application.Repositories;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.RequestVehicleModelCreation;

/// <summary>
/// Request to create or update models from specified make
/// </summary>
public class RequestVehicleModelCreationHandler
    : IRequestHandler<RequestVehicleModelCreationRequest, RequestVehicleModelCreationResponse>
{
    private readonly IEventBus _eventBus;
    private readonly VehicleContext _vehicleContext;

    public RequestVehicleModelCreationHandler(
        IEventBus eventBus,
        VehicleContext vehicleContext)
    {
        _eventBus = eventBus;
        _vehicleContext = vehicleContext;
    }

    public async Task<RequestVehicleModelCreationResponse> Handle(
        RequestVehicleModelCreationRequest request,
        CancellationToken cancellationToken)
    {
        var makeFound = await _vehicleContext
            .VehiclesMakes
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == request.MakeId);

        if (makeFound is null)
            throw new NotFoundCoreException("Vehicle make not found.");

        var @event = new CreateModelIntegrationEvent()
        {
            MakeId = request.MakeId
        };

        await _eventBus.PublishAsync(@event);

        var countAlreadyAdded = await _vehicleContext
            .VehiclesModels
            .AsNoTracking()
            .Where(m => m.VehicleMakeId == request.MakeId)
            .CountAsync();

        return new RequestVehicleModelCreationResponse(countAlreadyAdded);
    }
}

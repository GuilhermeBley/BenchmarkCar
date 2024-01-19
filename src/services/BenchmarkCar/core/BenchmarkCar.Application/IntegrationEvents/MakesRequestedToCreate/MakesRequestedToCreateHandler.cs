using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;

namespace BenchmarkCar.Application.IntegrationEvents.MakesRequestedToCreate;

public class MakesRequestedToCreateHandler
    : IIntegrationEventHandler<CreateMakesIntegrationEvent>
{
    private readonly VehicleContext _vehicleContext;
    private readonly IVehiclesDataQuery _vehicleDataQuery;
    private readonly ICoreLogger<MakesRequestedToCreateHandler> _logger;

    public MakesRequestedToCreateHandler(
        VehicleContext vehicleContext,
        IVehiclesDataQuery vehicleDataQuery,
        ICoreLogger<MakesRequestedToCreateHandler> logger)
    {
        _vehicleContext= vehicleContext;
        _vehicleDataQuery = vehicleDataQuery;
        _logger = logger;
    }

    public async Task Handle(
        CreateMakesIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        await foreach (var item in _vehicleDataQuery.GetAllMakesAsync())
            try
            {
                var vehicleMakeEntity
                    = VehicleMake.Create(
                        Guid.NewGuid(),
                        "",
                        externalId: "",
                        insertedAt: DateTimeOffset.UtcNow);

                await _vehicleContext.VehiclesMakes.AddAsync(
                    VehicleMakeModel.MapFromEntity(vehicleMakeEntity));

                await _vehicleContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add vehicle {0}.", item);
            }
    }
}

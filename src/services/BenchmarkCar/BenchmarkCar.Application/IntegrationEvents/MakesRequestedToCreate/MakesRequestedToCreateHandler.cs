using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.IntegrationEvents.MakesRequestedToCreate;

public class MakesRequestedToCreateHandler
    : IIntegrationEventHandler<CreateMakesIntegrationEvent>
{
    private readonly BenchmarkVehicleContext _vehicleContext;
    private readonly IVehiclesDataQuery _vehicleDataQuery;
    private readonly ICoreLogger<MakesRequestedToCreateHandler> _logger;

    public MakesRequestedToCreateHandler(
        BenchmarkVehicleContext vehicleContext,
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
        await foreach (var item in _vehicleDataQuery.GetAllMakesAsync()
            .WithCancellation(cancellationToken))
            try
            {
                var vehicleMakeEntity
                    = VehicleMake.Create(
                        id: Guid.NewGuid(),
                        name: item.MakeName,
                        externalId: item.MakeId,
                        insertedAt: DateTimeOffset.UtcNow);

                var vehicleAlreadyAdded
                    = await _vehicleContext
                    .VehiclesMakes
                    .AsNoTracking()
                    .Where(v => v.NormalizedName == vehicleMakeEntity.NormalizedName)
                    .FirstOrDefaultAsync(cancellationToken);

                if (vehicleAlreadyAdded is not null)
                {
                    _logger.LogInformation("Vehicle '{0}' have already been added.", vehicleAlreadyAdded.NormalizedName);
                    continue;
                }

                await _vehicleContext.VehiclesMakes.AddAsync(
                    VehicleMakeModel.MapFromEntity(vehicleMakeEntity));

                await _vehicleContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add vehicle '{0}'.", item.MakeId);
            }
    }
}

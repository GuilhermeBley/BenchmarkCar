using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.IntegrationEvents.CreateModelsByMake;

public class CreateModelsByMakeHandler
    : IIntegrationEventHandler<CreateModelsByMakeIntegrationEvent>
{
    private readonly IVehiclesDataQuery _api;
    private readonly ICoreLogger<CreateModelsByMakeHandler> _logger;
    private readonly VehicleContext _vehicleContext;

    public CreateModelsByMakeHandler(
        IVehiclesDataQuery api,
        ICoreLogger<CreateModelsByMakeHandler> logger,
        VehicleContext vehicleContext)
    {
        _api = api;
        _logger = logger;
        _vehicleContext = vehicleContext;
    }

    public async Task Handle(
        CreateModelsByMakeIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!Guid.TryParse(@event.MakeId, out var makeId))
        {
            throw new CommonCoreException("Invalid make id.");
        }

        var vehicleMake =
            await _vehicleContext
            .VehiclesMakes
            .AsNoTracking()
            .Where(v => v.Id == makeId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CommonCoreException("Invalid make id.");

        var vehicleMakeEntity = vehicleMake.MapToEntity();

        await foreach (var modelSummary in _api.GetByModelsSummaryByMakeAsync(vehicleMake.ExternalId)
            .WithCancellation(cancellationToken))
        {
            var vehicleFound =
                await _vehicleContext
                .VehiclesModels
                .AsNoTracking()
                .Where(v => v.NormalizedName == modelSummary.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (vehicleFound is not null)
            {
                _logger.LogInformation("Vehicle {0} already searched.", modelSummary.Name);
                continue;
            }

            VehicleModel? vehicleModelEntity = null;

            try
            {
                vehicleModelEntity =
                    VehicleModel.CreateNow(
                        vehicleMakeEntity,
                        modelSummary.Name,
                        modelSummary.Year,
                        modelSummary.Description,
                        modelSummary.ExternalId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create model summary.");
                continue;
            }

            using var transaction = 
                await _vehicleContext.Database.BeginTransactionAsync(cancellationToken);

            await _vehicleContext.VehiclesModels.AddAsync(
                VehicleModelModel.MapFromEntity(vehicleModelEntity));

            await transaction.CommitAsync(cancellationToken);

            await _vehicleContext.SaveChangesAsync(cancellationToken);   
        }
    }
}

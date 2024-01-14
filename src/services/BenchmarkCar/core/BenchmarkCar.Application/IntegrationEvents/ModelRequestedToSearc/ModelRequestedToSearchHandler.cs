using BenchmarkCar.Application.ExternalApi;
using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;

public class ModelRequestedToSearchHandler
    : IIntegrationEventHandler<CreateModelIntegrationEvent>
{
    private readonly IMediator _mediator;
    public readonly IVehiclesDataQuery _vehicleQuery;
    public readonly VehicleContext _vehicleContext;
    public readonly ICoreLogger _logger;

    public ModelRequestedToSearchHandler(
        IMediator mediator,
        IVehiclesDataQuery vehicleQuery, 
        VehicleContext vehicleContext, 
        ICoreLogger<ModelRequestedToSearchHandler> logger)
    {
        _mediator = mediator;
        _vehicleQuery = vehicleQuery;
        _vehicleContext = vehicleContext;
        _logger = logger;
    }

    public async Task Handle(
        CreateModelIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Trying to get '{0}' data details.", @event.ModelId);

        var engineData = await _vehicleContext.EngineModels
            .AsNoTracking()
            .Where(m => m.ModelId == @event.ModelId)
            .FirstOrDefaultAsync();

        if (engineData is not null)
        {
            _logger.LogInformation("Engine model '{0}' already added.", @event.ModelId);
            return;
        }

        var model = await _vehicleContext.VehiclesModels
            .AsNoTracking()
            .Where(m => m.Id == @event.ModelId)
            .FirstOrDefaultAsync();

        if (model is null)
        {
            _logger.LogInformation("Model '{0}' not found.", @event.ModelId);
            return;
        }

        var apiResult = await _vehicleQuery.GetByExternalModelId(modelId: @event.ModelId);

        if (apiResult.Engine is null &&
            apiResult.Body is null)
            throw new CommonCoreException("Invalid engine or body.");

        var vehicleModel = apiResult.Vehicle.MapToEntity();

        ModelEngine? modelEngine = null;
        ModelBody? modelBody = null;

        if (apiResult.Engine is not null)
            modelEngine = MapEngine(apiResult.Vehicle.Id, apiResult.Engine);

        if (apiResult.Body is not null)
            modelBody = MapBody(apiResult.Vehicle.Id, apiResult.Body);

        var vehicleFound =
            await _vehicleContext.VehiclesModels.FirstOrDefaultAsync(v => v.Id == apiResult.Vehicle.Id)
            ?? throw new NotFoundCoreException($"Vehicle model with id '{apiResult.Vehicle.Id}' was not found.");

        await using var transaction =
            await _vehicleContext.Database.BeginTransactionAsync(cancellationToken);

        ModelEngineModel? engineModelCreated = null;
        ModelBodyModel? bodyModelCreated = null;

        if (modelEngine is not null)
        {
            await ThrowIfAlreadyContainsModelEngineAsync(vehicleFound.Id);

            engineModelCreated = (await _vehicleContext.EngineModels.AddAsync(ModelEngineModel.MapFromEntity(modelEngine))).Entity;
        }

        if (modelBody is not null)
        {
            await ThrowIfAlreadyContainsModelBodyAsync(vehicleFound.Id);

            bodyModelCreated = (await _vehicleContext.ModelBodies.AddAsync(ModelBodyModel.MapFromEntity(modelBody))).Entity;
        }

        await transaction.CommitAsync(cancellationToken);
        await _vehicleContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ThrowIfAlreadyContainsModelEngineAsync(Guid modelId)
    {
        var vehicleFound = await _vehicleContext.EngineModels
            .FirstOrDefaultAsync(v => v.ModelId == modelId);

        if (vehicleFound is not null)
            throw new ConflictCoreException($"Engine with id '{modelId}' already exists.");
    }

    private async Task ThrowIfAlreadyContainsModelBodyAsync(Guid modelId)
    {
        var vehicleFound = await _vehicleContext.ModelBodies
            .FirstOrDefaultAsync(v => v.ModelId == modelId);

        if (vehicleFound is not null)
            throw new ConflictCoreException($"Body with id '{modelId}' already exists.");
    }

    private static ModelBody MapBody(Guid modelId, CreateBodyModel model)
        => ModelBody.Create(
            modelId: modelId,
            externalId: model.ExternalId,
            insertedAt: DateTimeOffset.UtcNow,
            doors: model.Door,
            seats: model.Seats,
            length: model.Length,
            width: model.Width,
            engineSize: model.EngineSize);

    private static ModelEngine MapEngine(Guid modelId, CreateEngineModel model)
        => ModelEngine.Create(
            modelId: modelId,
            insertedAt: DateTimeOffset.UtcNow,
            externalId: model.ExternalId,
            valves: model.Valves,
            horsePowerHp: model.HorsePowerHp,
            horsePowerRpm: model.HorsePowerRpm,
            torqueFtLbs: model.TorqueFtLbs,
            torqueRpm: model.TorqueRpm);
}

using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

public class CreateVehicleModelDetailsHandler
    : IRequestHandler<CreateVehicleModelDetailsRequest, CreateVehicleModelDetailsResponse>
{
    private readonly BenchmarkVehicleContext _vehicleContext;
    private readonly ICoreLogger _logger;

    public CreateVehicleModelDetailsHandler(
        BenchmarkVehicleContext vehicleContext, 
        ICoreLogger<CreateVehicleModelDetailsHandler> logger)
    {
        _vehicleContext = vehicleContext;
        _logger = logger;
    }

    public async Task<CreateVehicleModelDetailsResponse> Handle(
        CreateVehicleModelDetailsRequest request, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Trying to get '{0}' insert details.", request.ModelId);

        var vehicleModel = await _vehicleContext.VehiclesModels
            .AsNoTracking()
            .Where(m => m.Id == request.ModelId)
            .FirstOrDefaultAsync();

        if (vehicleModel is not null)
            vehicleModel.VehicleMake
                = await _vehicleContext
                .VehiclesMakes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == vehicleModel.VehicleMakeId);

        if (vehicleModel is null ||
            vehicleModel.VehicleMake is null)
        {
            _logger.LogInformation("Model '{0}' not found.", request.ModelId);
            throw new NotFoundCoreException("Model not found.");
        }

        var vehicleEntity = vehicleModel.MapToEntity();

        ModelEngine? modelEngine = null;
        ModelBody? modelBody = null;

        if (request.Engine is not null)
            modelEngine = MapEngine(vehicleEntity.Id, request.Engine);

        if (request.Body is not null)
            modelBody = MapBody(vehicleEntity.Id, request.Body);

        await using var transaction =
            await _vehicleContext.Database.BeginTransactionAsync(cancellationToken);

        ModelEngineModel? engineModelCreated = null;
        ModelBodyModel? bodyModelCreated = null;

        if (modelEngine is not null)
        {
            var containsEngine = await ContainsModelEngineAsync(vehicleEntity.Id);

            if (!containsEngine)
                engineModelCreated = (await _vehicleContext.EngineModels.AddAsync(ModelEngineModel.MapFromEntity(modelEngine))).Entity;
        }

        if (modelBody is not null)
        {
            var containsBody = await ContainsModelBodyAsync(vehicleEntity.Id);

            if (!containsBody)
                bodyModelCreated = (await _vehicleContext.ModelBodies.AddAsync(ModelBodyModel.MapFromEntity(modelBody))).Entity;
        }

        await transaction.CommitAsync(cancellationToken);

        await _vehicleContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Engine '{0}' and '{1}' was successfully added.", engineModelCreated?.ExternalId, bodyModelCreated?.ExternalId);
        return new();
    }

    private Task<bool> ContainsModelEngineAsync(Guid modelId)
        => _vehicleContext.EngineModels
            .AnyAsync(v => v.ModelId == modelId);

    private Task<bool> ContainsModelBodyAsync(Guid modelId)
        => _vehicleContext.ModelBodies
            .AnyAsync(v => v.ModelId == modelId);

    private static ModelBody MapBody(Guid modelId, CreateBodyModel model)
        => ModelBody.Create(
            modelId: modelId,
            externalId: model.ExternalId,
            insertedAt: DateTimeOffset.UtcNow,
            doors: model.Door,
            seats: model.Seats,
            length: model.Length,
            width: model.Width);

    private static ModelEngine MapEngine(Guid modelId, CreateEngineModel model)
        => ModelEngine.Create(
            modelId: modelId,
            insertedAt: DateTimeOffset.UtcNow,
            externalId: model.ExternalId,
            valves: model.Valves,
            horsePowerHp: model.HorsePowerHp,
            horsePowerRpm: model.HorsePowerRpm,
            torqueFtLbs: model.TorqueFtLbs,
            torqueRpm: model.TorqueRpm,
            engineSize: model.EngineSize);
}

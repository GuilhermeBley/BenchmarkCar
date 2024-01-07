using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

/// <summary>
/// Add a new vehicle model details, the data is about model and engine.
/// </summary>
/// <remarks>
///     <para>Try add model to best models.</para>
/// </remarks>
/// <exception cref="ConflictCoreException"></exception>
/// <exception cref="NotFoundCoreException"></exception>
/// <exception cref="CommonCoreException"></exception>
public class CreateVehicleModelDetailsHandler
    : IRequestHandler<CreateVehicleModelDetailsRequest, CreateVehicleModelDetailsResponse>
{
    private readonly VehicleContext _vehicleContext;

    public CreateVehicleModelDetailsHandler(
        VehicleContext vehicleContext)
    {
        _vehicleContext = vehicleContext;
    }

    public async Task<CreateVehicleModelDetailsResponse> Handle(
        CreateVehicleModelDetailsRequest request, 
        CancellationToken cancellationToken)
    {
        if (request.Engine is null &&
            request.Body is null)
            return CreateVehicleModelDetailsResponse.Default;

        var vehicleModel = request.Vehicle.MapToEntity();

        ModelEngine? modelEngine = null;
        ModelBody? modelBody = null;

        if (request.Engine is not null)
            modelEngine = MapEngine(request.Vehicle.Id, request.Engine);

        if (request.Body is not null)
            modelBody = MapBody(request.Vehicle.Id, request.Body);

        var vehicleFound =
            await _vehicleContext.VehiclesModels.FirstOrDefaultAsync(v => v.Id == request.Vehicle.Id)
            ?? throw new NotFoundCoreException($"Vehicle model with id '{request.Vehicle.Id}' was not found.");

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

        return new CreateVehicleModelDetailsResponse(
            EngineIdCreatedOrUpdated: engineModelCreated?.ModelId,
            BodyIdCreatedOrUpdated: engineModelCreated?.ModelId);
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

using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

/// <summary>
/// Add a new vehicle model details, the data is about model and engine.
/// </summary>
/// <remarks>
///     <para>Try add model to best models.</para>
/// </remarks>
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
            modelEngine = MapEngine(request.Engine);

        if (request.Body is not null)
            modelBody = MapBody(request.Body);

        await using var transaction =
            await _vehicleContext.Database.BeginTransactionAsync(cancellationToken);

        ModelEngineModel? engineModelCreated = null;
        ModelBodyModel? bodyModelCreated = null;

        if (modelEngine is not null)
        {
            engineModelCreated = (await _vehicleContext.EngineModels.AddAsync(ModelEngineModel.MapFromEntity(modelEngine))).Entity;
        }

        if (modelBody is not null)
        {
            bodyModelCreated = (await _vehicleContext.ModelBodies.AddAsync(ModelBodyModel.MapFromEntity(modelBody))).Entity;
        }

        await transaction.CommitAsync(cancellationToken);
        await _vehicleContext.SaveChangesAsync(cancellationToken);

        return new CreateVehicleModelDetailsResponse(
            EngineIdCreatedOrUpdated: engineModelCreated?.ModelId,
            BodyIdCreatedOrUpdated: engineModelCreated?.ModelId);
    }
}

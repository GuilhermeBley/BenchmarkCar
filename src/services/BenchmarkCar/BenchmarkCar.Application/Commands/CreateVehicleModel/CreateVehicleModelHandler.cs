using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.CreateVehicleModel;

public class CreateVehicleModelHandler
    : IRequestHandler<CreateVehicleModelRequest, CreateVehicleModelResponse>
{
    private readonly VehicleContext _vehicleContext;

    public CreateVehicleModelHandler(VehicleContext vehicleContext)
    {
        _vehicleContext = vehicleContext;
    }

    public async Task<CreateVehicleModelResponse> Handle(
        CreateVehicleModelRequest request, 
        CancellationToken cancellationToken)
    {
        var vehicleMake = request.VehicleMake.MapToEntity();

        var vehicleModel = VehicleModel.CreateNow(
            vehicleMake, 
            request.Name, 
            request.Year,
            request.Description, 
            request.ExternalId);

        await using var transaction 
            = await _vehicleContext.Database.BeginTransactionAsync(cancellationToken);

        var vehicleWithSameName
            = await _vehicleContext.VehiclesModels
                .FirstOrDefaultAsync(vm => vm.NormalizedName == vehicleModel.NormalizedName, cancellationToken);

        if (vehicleWithSameName is not null)
            throw new ConflictCoreException($"Model with name '{vehicleWithSameName.NormalizedName}' already exists.");

        var result = await _vehicleContext.VehiclesModels.AddAsync(
            VehicleModelModel.MapFromEntity(vehicleModel));

        await transaction.CommitAsync(cancellationToken);
        await _vehicleContext.SaveChangesAsync(cancellationToken);

        return new CreateVehicleModelResponse(
            Id: result.Entity.Id,
            VehicleMakeId: result.Entity.VehicleMakeId,
            Name: result.Entity.Name,
            NormalizedName: result.Entity.NormalizedName,
            Description: result.Entity.Description,
            ExternalId: result.Entity.ExternalId,
            InsertedAt: result.Entity.InsertedAt);
    }
}

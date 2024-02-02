using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.CreateVehicleMake;

public class CreateVehicleMakeHandler
    : IRequestHandler<CreateVehicleMakeRequest, CreateVehicleMakeResponse>
{
    private readonly VehicleContext _vehicleContext;

    public CreateVehicleMakeHandler(
        VehicleContext vehicleContext)
    {
        _vehicleContext = vehicleContext;
    }

    public async Task<CreateVehicleMakeResponse> Handle(
        CreateVehicleMakeRequest request, 
        CancellationToken cancellationToken)
    {
        var vehicleEntity = VehicleMake.Create(
            Guid.NewGuid(),
            request.Name,
            request.ExternalId,
            DateTimeOffset.UtcNow);

        var modelToAdd = VehicleMakeModel.MapFromEntity(vehicleEntity);

        await using var transaction = 
            await _vehicleContext.Database.BeginTransactionAsync(cancellationToken);

        var vehicleWithSameName
            = await _vehicleContext.VehiclesMakes.FirstOrDefaultAsync(e => e.NormalizedName == vehicleEntity.NormalizedName);

        if (vehicleWithSameName is not null)
            throw new ConflictCoreException("Vehicle already added.");

        var result = await _vehicleContext.VehiclesMakes.AddAsync(modelToAdd);

        await _vehicleContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync();

        return new CreateVehicleMakeResponse(
            Id: result.Entity.Id,
            NormalizedName: result.Entity.NormalizedName,
            Name: result.Entity.Name,
            ExternalId: result.Entity.ExternalId,
            InsertedAt: result.Entity.InsertedAt);
    }
}

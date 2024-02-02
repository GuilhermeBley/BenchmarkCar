using BenchmarkCar.Domain.Entities.Vehicles;

namespace BenchmarkCar.Application.Model.Vehicles;

public class VehicleModelModel
{
    public Guid Id { get; set; }
    public Guid VehicleMakeId { get; set; }
    public VehicleMakeModel VehicleMake { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public string NormalizedName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public DateTimeOffset InsertedAt { get; set; }

    public VehicleModel MapToEntity()
        => VehicleModel.Create(
            id: Id,
            vehicleMake: VehicleMake.MapToEntity(),
            name: Name,
            year: Year,
            description: Description,
            externalId: ExternalId,
            insertedAt: InsertedAt);

    public static VehicleModelModel MapFromEntity(VehicleModel entity)
        => new()
        {
            Description = entity.Description,
            ExternalId = entity.ExternalId.ToString() ?? string.Empty,
            Name = entity.Name,
            NormalizedName = entity.NormalizedName,
            Id = entity.Id,
            Year = entity.Year,
            InsertedAt = entity.InsertedAt,
            VehicleMakeId = entity.Make.Id
        };
}

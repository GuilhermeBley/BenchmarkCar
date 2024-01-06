using BenchmarkCar.Domain.Entities.Vehicles;

namespace BenchmarkCar.Application.Model.Vehicles;

public  class VehicleMakeModel
{
    public Guid Id { get; set; }
    public string NormalizedName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public DateTimeOffset InsertedAt { get; set; }

    public VehicleMake MapToEntity()
        => VehicleMake.Create(
            id: Id,
            name: Name,
            externalId: ExternalId,
            insertedAt: InsertedAt);

    public static VehicleMakeModel MapFromEntity(VehicleMake entity)
        => new()
        {
            Id = entity.Id,
            ExternalId = entity.ExternalId.ToString() ?? string.Empty,
            NormalizedName = entity.NormalizedName,
            Name = entity.Name,
            InsertedAt = entity.InsertedAt
        };
}

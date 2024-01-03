using BenchmarkCar.Domain.Entities.Vehicles;

namespace BenchmarkCar.Application.Model.Vehicles;

public class VehicleModelModel
{
    public Guid Id { get; set; }
    public Guid VehicleMakeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public DateTimeOffset InsertedAt { get; set; }

    public static VehicleModelModel MapFromEntity(VehicleModel entity)
        => new()
        {
            Description = entity.Description,
            ExternalId = entity.ExternalId.ToString() ?? string.Empty,
            Name = entity.Name,
            NormalizedName = entity.NormalizedName,
            Id = entity.Id,
            InsertedAt = entity.InsertedAt,
            VehicleMakeId = entity.Make.Id
        };
}

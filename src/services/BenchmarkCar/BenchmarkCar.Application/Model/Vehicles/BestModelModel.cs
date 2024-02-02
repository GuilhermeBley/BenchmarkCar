using BenchmarkCar.Domain.Entities.Vehicles;

namespace BenchmarkCar.Application.Model.Vehicles;

public class BestModelModel
{
    public Guid Id { get; set; }
    public Guid VehicleModelId { get; set; }
    public string Area { get; set; } = string.Empty;
    public DateTimeOffset InsertedAt { get; set; }

    public static BestModelModel MapFromEntity(BestModel entity)
        => new()
        {
            Id = entity.Id,
            InsertedAt = entity.InsertedAt,
            Area = entity.Area,
            VehicleModelId = entity.Vehicle.Id
        };
}

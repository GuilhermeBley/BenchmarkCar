using System;

namespace BenchmarkCar.Domain.Entities.Vehicles;

public record BestModel(
    Guid Id,
    VehicleModel Vehicle,
    string Area,
    DateTimeOffset InsertedAt)
    : RecordEntity
{
    protected override void ValidEntity()
    {
        base.ValidEntity();

        if (Area.Length > 255)
            throw new CommonCoreException("Invalid 'area' length.");
    }

    public static BestModel CreateNow(
        VehicleModel Vehicle,
        string Area)
        => new BestModel(
            Guid.NewGuid(),
            Vehicle,
            Area.Trim('\n', '\t', ' ').ToUpperInvariant(),
            DateTimeOffset.UtcNow);
}

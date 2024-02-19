using BenchmarkCar.Domain.Entities.Vehicles;

namespace BenchmarkCar.Application.Model.Vehicles;

public class ModelEngineModel
{
    public Guid ModelId { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public DateTimeOffset InsertedAt { get; set; }
    public decimal? Valves { get; set; }
    public decimal? HorsePowerHp { get; set; }
    public decimal? HorsePowerRpm { get; set; }
    public decimal? TorqueFtLbs { get; set; }
    public decimal? TorqueRpm { get; set; }
    public decimal? EngineSize { get; set; }

    public static ModelEngineModel MapFromEntity(ModelEngine entity)
        => new()
        {
            ExternalId = entity.ExternalId.ToString() ?? string.Empty,
            HorsePowerHp = entity.HorsePowerHp,
            HorsePowerRpm = entity.HorsePowerRpm,
            InsertedAt = entity.InsertedAt,
            Valves = entity.Valves,
            ModelId = entity.ModelId,
            TorqueFtLbs = entity.TorqueFtLbs,
            TorqueRpm = entity.TorqueRpm,
            EngineSize = entity.EngineSize,
        };
}

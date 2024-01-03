using BenchmarkCar.Domain.Entities.Vehicles;

namespace BenchmarkCar.Application.Model.Vehicles;

public class ModelBodyModel
{
    public Guid ModelId { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public DateTimeOffset InsertedAt { get; set; }
    public int Doors { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public int Seats { get; set; }
    public decimal? EngineSize { get; set; }

    public static ModelBodyModel MapFromEntity(ModelBody entity)
        => new()
        {
            ModelId = entity.ModelId,
            Doors = entity.Doors,
            EngineSize = entity.EngineSize,
            Seats = entity.Seats,
            Width = entity.Width,
            ExternalId = entity.ExternalId.ToString() ?? string.Empty,
            InsertedAt = entity.InsertedAt,
            Length = entity.Length,
        };
}

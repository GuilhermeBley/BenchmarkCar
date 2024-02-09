using BenchmarkCar.Domain.Entities.Queue;

namespace BenchmarkCar.Application.Model.Queue;

public class ProcessingStateModel
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public double Percent { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;

    public static ProcessingStateModel MapFrom(
        ProcessingState entity)
        => new()
        {
            Id = entity.Id,
            Area = entity.Area,
            Key = entity.Key,
            Code = (int)entity.Code,
            Percent = entity.Percent,
        };
}

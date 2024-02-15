using BenchmarkCar.Domain.Entities.Queue;
using System.Collections.Immutable;
using System.Text.Json;

namespace BenchmarkCar.Application.Model.Queue;

public class ProcessingStateModel
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public double Percent { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? MetaData { get; set; } = string.Empty;

    public T? ParseMetaData<T>()
        => string.IsNullOrEmpty(MetaData) ?
        default :
        JsonSerializer.Deserialize<T>(MetaData)
        ?? throw new CommonCoreException("Failed to parse data.");

    public IReadOnlyDictionary<string, object> ParseMetaData()
        => string.IsNullOrEmpty(MetaData) ?
        ImmutableDictionary<string,object>.Empty :
        JsonSerializer.Deserialize<IReadOnlyDictionary<string, object>>(MetaData)
        ?? throw new CommonCoreException("Failed to parse data.");

    public ProcessingState MapToEntity()
        => ProcessingState.Create(
            Id,
            Percent,
            Area,
            Key,
            ParseMetaData());

    public static ProcessingStateModel MapFrom(
        ProcessingState entity)
        => new()
        {
            Id = entity.Id,
            Area = entity.Area,
            Key = entity.Key,
            Code = (int)entity.Code,
            Percent = entity.Percent,
            MetaData = JsonSerializer.Serialize(entity.MetaData)
        };
}

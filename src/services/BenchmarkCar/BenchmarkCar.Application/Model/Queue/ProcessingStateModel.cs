using BenchmarkCar.Domain.Entities.Queue;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BenchmarkCar.Application.Model.Queue;

public class ProcessingStateModel
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public double Percent { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string MetaData { get; set; } = string.Empty;

    public T ParseMetaData<T>()
        => JsonSerializer.Deserialize<T>(MetaData)
        ?? throw new CommonCoreException("Failed to parse data.");

    public IReadOnlyDictionary<string, object> ParseMetaData()
        => JsonSerializer.Deserialize<IReadOnlyDictionary<string, object>>(MetaData)
        ?? throw new CommonCoreException("Failed to parse data.");

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

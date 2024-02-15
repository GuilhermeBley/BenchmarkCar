using BenchmarkCar.Domain.Entities.Queue;
using System.Text.Json;

namespace BenchmarkCar.Application.Model.Queue;

public class ProcessingResultModel
{
    public Guid LinkedProccessId { get; set; }
    public string? Data { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }

    public T? TryParseData<T>() 
        where T : class
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Data))
                return null;

            return JsonSerializer.Deserialize<T>(Data);
        }
        catch
        {
            return null;
        }
    }
    
    public static ProcessingResultModel MapFrom(
        ProcessingResult entity)
    {
        return new ProcessingResultModel
        {
            Data = entity.Data is null ?
                null :
                JsonSerializer.Serialize(entity.Data),
            ExpirationDate = entity.ExpirationDate,
            LinkedProccessId = entity.LinkedProccessId
        };
    }
}

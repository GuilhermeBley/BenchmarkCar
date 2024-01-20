using System.Text.Json.Serialization;

namespace BenchmarkCar.Infrastructure.Model.CarApi;

internal class VehicleMakeResponse
{
    [JsonPropertyName("Data")]
    public IEnumerable<VehicleMakeResponseDatum> Data { get; set; } 
        = Enumerable.Empty<VehicleMakeResponseDatum>();
}

internal class VehicleMakeResponseDatum
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

using System.Text.Json.Serialization;

namespace BenchmarkCar.Infrastructure.Model.CarApi;

internal class Datum
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("make_model_id")]
    public int? MakeModelId { get; set; }

    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("msrp")]
    public int? Msrp { get; set; }

    [JsonPropertyName("invoice")]
    public int? Invoice { get; set; }

    [JsonPropertyName("created")]
    public DateTime? Created { get; set; }

    [JsonPropertyName("modified")]
    public DateTime? Modified { get; set; }
}

internal class VehicleTrimSummaryResponse
{
    [JsonPropertyName("data")]
    public IEnumerable<Datum> Data { get; set; }
         = Enumerable.Empty<Datum>();
}



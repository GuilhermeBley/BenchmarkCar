namespace BenchmarkCar.Infrastructure.Options;

internal class CarApiOptions
{
    public const string SECTION = "CarApiOptions";

    public string ApiToken { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}

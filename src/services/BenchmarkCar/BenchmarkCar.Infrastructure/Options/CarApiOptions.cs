using System.ComponentModel.DataAnnotations;

namespace BenchmarkCar.Infrastructure.Options;

public class CarApiOptions
{
    public const string SECTION = "CarApiOptions";

    [Required]
    [MinLength(2)]
    public string ApiToken { get; set; } = string.Empty;
    [Required]
    [MinLength(2)]
    public string ApiSecret { get; set; } = string.Empty;
}

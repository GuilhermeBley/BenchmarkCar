using System.ComponentModel.DataAnnotations;

namespace BenchmarkCar.Infrastructure.Options;

public class SqlOptions
{
    public const string SECTION = "SqlServer";

    [Required]
    [MinLength(10)]
    public string ConnectionString { get; init; } = string.Empty;
}

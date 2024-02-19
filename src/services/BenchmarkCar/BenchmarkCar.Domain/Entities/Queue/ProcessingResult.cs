namespace BenchmarkCar.Domain.Entities.Queue;

public sealed record ProcessingResult(
    Guid LinkedProccessId,
    object? Data,
    DateTimeOffset ExpirationDate)
    : RecordEntity
{
    protected override void ValidEntity()
    {
        base.ValidEntity();

        if (!IsValidadExpirationDate(ExpirationDate))
        {
            throw new CommonCoreException("Expiration date should be at min 1 minute of lifetime.");
        }
    }

    public static bool IsValidadExpirationDate(DateTimeOffset date)
        => DateTimeOffset.UtcNow.AddMinutes(1) < date;
}

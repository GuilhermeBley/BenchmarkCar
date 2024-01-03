namespace BenchmarkCar.Domain.Entities;

public record RecordEntity : IEntity
{
    public virtual Guid EntityId { get; } = Guid.NewGuid();

    public RecordEntity()
        => ValidEntity();

    protected virtual void ValidEntity()
    {
    }
}

namespace BenchmarkCar.Domain.Entities;

public abstract class Entity 
    : IEntity
{
    public virtual Guid EntityId { get; }

    public override bool Equals(object? obj)
    {
        return obj is Entity entity &&
               EntityId.Equals(entity.EntityId);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EntityId);
    }
}

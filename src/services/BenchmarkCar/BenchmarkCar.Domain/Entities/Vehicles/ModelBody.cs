namespace BenchmarkCar.Domain.Entities.Vehicles;

public class ModelBody
    : Entity
{
    public Guid ModelId { get; private set; }
    public object ExternalId { get; private set; }
    public DateTimeOffset InsertedAt { get; private set; }
    public int Doors { get; private set; }
    public decimal? Length { get; private set; }
    public decimal? Width { get; private set; }
    public int Seats { get; private set; }

    private ModelBody(
        Guid modelId, 
        object externalId, 
        DateTimeOffset insertedAt, 
        int doors, 
        decimal? length, 
        decimal? width, 
        int seats)
    {
        ModelId = modelId;
        ExternalId = externalId;
        InsertedAt = insertedAt;
        Doors = doors;
        Length = length;
        Width = width;
        Seats = seats;
    }

    public override bool Equals(object? obj)
    {
        return obj is ModelBody body &&
               base.Equals(obj) &&
               EntityId.Equals(body.EntityId) &&
               ModelId.Equals(body.ModelId) &&
               EqualityComparer<object>.Default.Equals(ExternalId, body.ExternalId) &&
               InsertedAt.Equals(body.InsertedAt) &&
               Doors == body.Doors &&
               Length == body.Length &&
               Width == body.Width &&
               Seats == body.Seats;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(ModelId);
        hash.Add(ExternalId);
        hash.Add(InsertedAt);
        hash.Add(Doors);
        hash.Add(Length);
        hash.Add(Width);
        hash.Add(Seats);
        return hash.ToHashCode();
    }

    public static ModelBody Create(
        Guid modelId,
        object externalId,
        DateTimeOffset insertedAt,
        int doors,
        int seats,
        decimal? length = null,
        decimal? width = null,
        decimal? engineSize = null)
    {
        if (doors < 1)
            throw new CommonCoreException("Vehicles do not have less than one doors.");

        if (seats < 1)
            throw new CommonCoreException("Vehicles do not have less than one seats.");

        if (length is not null && length < 0)
            throw new CommonCoreException("Vehicles do not have less than zero length.");

        if (width is not null && width < 0)
            throw new CommonCoreException("Vehicles do not have less than zero width width.");

        if (engineSize is not null && engineSize < 0)
            throw new CommonCoreException("Vehicles do not have less than zero engine size.");

        return new ModelBody(
            modelId: modelId,
            externalId: externalId,
            insertedAt: insertedAt,
            doors: doors,
            length: length,
            width: width,
            seats: seats);
    }
}

namespace BenchmarkCar.Domain.Entities.Vehicles;

public class Make
    : Entity
{
    private const int MAX_NAME_LENGTH = 45;
    private const int MAX_EXTERNALID_LENGTH = 45;

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; }
    public object ExternalId { get; private set; }
    public DateTimeOffset InsertedAt { get; private set; }

    private Make(Guid id, string name, string normalizedName, object externalId, DateTimeOffset insertedAt)
    {
        Id = id;
        Name = name;
        NormalizedName = normalizedName;
        ExternalId = externalId;
        InsertedAt = insertedAt;
    }

    public override bool Equals(object? obj)
    {
        return obj is Make make &&
               base.Equals(obj) &&
               EntityId.Equals(make.EntityId) &&
               Id.Equals(make.Id) &&
               Name == make.Name &&
               NormalizedName == make.NormalizedName &&
               EqualityComparer<object>.Default.Equals(ExternalId, make.ExternalId) &&
               InsertedAt.Equals(make.InsertedAt);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), EntityId, Id, Name, NormalizedName, ExternalId, InsertedAt);
    }

    public static Make Create<TExternalId>(
        Guid id, 
        string name,
        TExternalId externalId, 
        DateTimeOffset insertedAt)
    {
        if (string.IsNullOrEmpty(name) ||
            name.Length > MAX_NAME_LENGTH)
            throw new CommonCoreException("Invalid name.");

        if (externalId is null)
            throw new CommonCoreException("Invalid externalId.");

        var externalIdText = externalId.ToString();

        if (string.IsNullOrEmpty(externalIdText) ||
            externalIdText.Length > MAX_EXTERNALID_LENGTH)
            throw new CommonCoreException("externalIdText is invalid.");

        return new Make(
            id,
            name,
            name.ToUpperInvariant(),
            externalIdText,
            insertedAt);
    }
}

namespace BenchmarkCar.Domain.Entities.Vehicles;

public class VehicleModel
    : Entity
{
    private const int MAX_DESCRIPTION_LENGTH = 5000;
    private const int MAX_NAME_LENGTH = 255;

    public Guid Id { get; private set; } 
    public VehicleMake Make { get; private set; } 
    public string Name { get; private set; }
    public string NormalizedName { get; private set; }
    public string? Description { get; private set; }
    public object ExternalId { get; private set; }
    public DateTimeOffset InsertedAt { get; private set; }

    public VehicleModel(Guid id, VehicleMake vehicleMake, string name, string normalizedName, string? description, object externalId, DateTimeOffset insertedAt)
    {
        Id = id;
        Make = vehicleMake;
        Name = name;
        NormalizedName = normalizedName;
        Description = description;
        ExternalId = externalId;
        InsertedAt = insertedAt;
    }

    public override bool Equals(object? obj)
    {
        return obj is VehicleModel model &&
               base.Equals(obj) &&
               EntityId.Equals(model.EntityId) &&
               Id.Equals(model.Id) &&
               EqualityComparer<VehicleMake>.Default.Equals(Make, model.Make) &&
               Name == model.Name &&
               NormalizedName == model.NormalizedName &&
               Description == model.Description &&
               EqualityComparer<object>.Default.Equals(ExternalId, model.ExternalId) &&
               InsertedAt == model.InsertedAt;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(Id);
        hash.Add(Make);
        hash.Add(Name);
        hash.Add(NormalizedName);
        hash.Add(Description);
        hash.Add(ExternalId);
        hash.Add(InsertedAt);
        return hash.ToHashCode();
    }

    public static VehicleModel CreateNow(
        VehicleMake vehicleMake,
        string name,
        string? description,
        object externalId)
        => Create(
            Guid.NewGuid(),
            vehicleMake,
            name,
            description,
            externalId,
            DateTimeOffset.UtcNow);

    public static VehicleModel Create(
        Guid id, 
        VehicleMake vehicleMake, 
        string name, 
        string? description, 
        object externalId, 
        DateTimeOffset insertedAt)
    {
        if (vehicleMake is null)
            throw new CommonCoreException("Vehicle make is null.");

        if (string.IsNullOrEmpty(name))
            throw new CommonCoreException("Invalid name, it isn't allowed null or empty.");

        name = name.Trim(' ', '\t', '\n');
        description = description?.Trim(' ', '\t', '\n');

        if (name.Length > MAX_NAME_LENGTH)
            throw new CommonCoreException($"Invalid name, the max length is '{MAX_NAME_LENGTH}'.");

        if (description is not null &&
            description.Length > MAX_DESCRIPTION_LENGTH)
            throw new CommonCoreException($"Invalid description, the max length is '{MAX_DESCRIPTION_LENGTH}'.");

        if (externalId is null)
            throw new CommonCoreException("Invalid externalId, it isn't allowed null.");

        return new VehicleModel(
            id: id,
            vehicleMake: vehicleMake,
            name: name,
            normalizedName: name.ToUpperInvariant(),
            description: description,
            externalId: externalId,
            insertedAt: insertedAt);
    }
}

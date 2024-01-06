namespace BenchmarkCar.Domain.Entities.Vehicles;

public class VehicleModel
    : Entity
{
    private const int MAX_DESCRIPTION_LENGTH = 5000;
    private const int MAX_NAME_LENGTH = 255;
    private const int MAX_NORMALIZED_NAME_LENGTH = 255;
    private const int MIN_NAME_LENGTH = 2;
    private static int MaxModelYear 
        => DateTime.UtcNow.Year + 1;

    public Guid Id { get; private set; } 
    public VehicleMake Make { get; private set; } 
    public string Name { get; private set; }
    public int Year { get; private set; }
    public string NormalizedName { get; private set; }
    public string? Description { get; private set; }
    public object ExternalId { get; private set; }
    public DateTimeOffset InsertedAt { get; private set; }

    private VehicleModel(
        Guid id, 
        VehicleMake vehicleMake, 
        string name, 
        string normalizedName, 
        string? description, 
        object externalId, 
        DateTimeOffset insertedAt, int year)
    {
        Id = id;
        Make = vehicleMake;
        Name = name;
        NormalizedName = normalizedName;
        Description = description;
        ExternalId = externalId;
        InsertedAt = insertedAt;
        Year = year;
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
        int year,
        string? description,
        object externalId)
        => Create(
            Guid.NewGuid(),
            vehicleMake,
            name,
            year,
            description,
            externalId,
            DateTimeOffset.UtcNow);

    public static VehicleModel Create(
        Guid id, 
        VehicleMake vehicleMake, 
        string name, 
        int year, 
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

        if (name.Length > MAX_DESCRIPTION_LENGTH ||
            name.Length < MIN_NAME_LENGTH)
            throw new CommonCoreException($"Invalid name, the string length is '{MIN_NAME_LENGTH}' to '{MAX_NAME_LENGTH}'.");

        if (description is not null &&
            description.Length > MAX_DESCRIPTION_LENGTH)
            throw new CommonCoreException($"Invalid description, the max length is '{MAX_DESCRIPTION_LENGTH}'.");

        if (externalId is null)
            throw new CommonCoreException("Invalid externalId, it isn't allowed null.");

        if (year > MaxModelYear)
            throw new CommonCoreException("Invalid model year.");

        var normalizedName = string.Concat(
            vehicleMake.NormalizedName,
            '-',
            name.ToUpperInvariant(),
            '-',
            year);

        if (normalizedName.Length > MAX_NORMALIZED_NAME_LENGTH)
            throw new CommonCoreException("Too long normalized name.");

        return new VehicleModel(
            id: id,
            vehicleMake: vehicleMake,
            name: name,
            year: year,
            normalizedName: name.ToUpperInvariant(),
            description: description,
            externalId: externalId,
            insertedAt: insertedAt);
    }
}

namespace BenchmarkCar.Domain.Entities.Vehicles;

/// <summary>
/// Relationship between vehicle model and his own engine
/// </summary>
public class ModelEngine
    : Entity
{
    public Guid ModelId { get; set; }
    public object ExternalId { get; set; }
    public DateTimeOffset InsertedAt { get; set; }

    /// <summary>
    /// Vehicle valves
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Valves in an engine manage the flow of air and fuel in and out of the combustion chamber. 
    ///     </para>
    /// </remarks>
    public decimal? Valves { get; private set; }

    /// <summary>
    /// Horsepower (HP) measures an engine's work rate
    /// </summary>
    /// <remarks>
    ///     <para>
    /// It's the power an engine produces, often indicating 
    /// performance in vehicles. 1 HP equals 746 watts or the force to lift 550 pounds by one foot in a second. 
    /// Higher HP generally means better performance. It's crucial for understanding an engine's power output, 
    /// used widely for comparing vehicle capabilities and industrial machinery.
    ///     </para>
    /// </remarks>
    public decimal? HorsePowerHp { get; private set; }

    /// <summary>
    /// engine's power output at a specific engine speed or revolutions per minute
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     In the context of vehicles, horsepower represents the engine's capability to perform work, 
    ///     specifically the power output of the engine.
    ///     "Horsepower at RPM" refers to the measurement of the engine's power output at a specific engine speed or revolutions per minute (RPM).
    ///     </para>
    /// </remarks>
    public decimal? HorsePowerRpm { get; private set; }

    /// <summary>
    /// "Torque in ft-lbs" refers to the engine's twisting force, indicating how much rotational force it can apply at a specific point on the crankshaft
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Torque, measured in foot-pounds (ft-lbs) or Newton-meters (Nm), is a crucial aspect of an engine's performance. Torque represents the 
    ///     rotational force produced by the engine and is often mentioned alongside horsepower to provide a comprehensive view of an engine's capabilities.
    ///     </para>
    /// </remarks>
    public decimal? TorqueFtLbs { get; private set; }

    /// <summary>
    /// The same of <see cref="TorqueFtLbs"/>, but the way to measure is in speed or revolutions per minute
    /// </summary>
    public decimal? TorqueRpm { get; private set; }

    private ModelEngine(
        Guid modelId, 
        object externalId, 
        DateTimeOffset insertedAt, 
        decimal? valves, 
        decimal? horsePowerHp, 
        decimal? horsePowerRpm, 
        decimal? torqueFtLbs, 
        decimal? torqueRpm)
    {
        ModelId = modelId;
        ExternalId = externalId;
        InsertedAt = insertedAt;
        Valves = valves;
        HorsePowerHp = horsePowerHp;
        HorsePowerRpm = horsePowerRpm;
        TorqueFtLbs = torqueFtLbs;
        TorqueRpm = torqueRpm;
    }

    public override bool Equals(object? obj)
    {
        return obj is ModelEngine engine &&
               base.Equals(obj) &&
               EntityId.Equals(engine.EntityId) &&
               ModelId.Equals(engine.ModelId) &&
               EqualityComparer<object>.Default.Equals(ExternalId, engine.ExternalId) &&
               InsertedAt.Equals(engine.InsertedAt) &&
               Valves == engine.Valves &&
               HorsePowerHp == engine.HorsePowerHp &&
               HorsePowerRpm == engine.HorsePowerRpm &&
               TorqueFtLbs == engine.TorqueFtLbs &&
               TorqueRpm == engine.TorqueRpm;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(ModelId);
        hash.Add(ExternalId);
        hash.Add(InsertedAt);
        hash.Add(Valves);
        hash.Add(HorsePowerHp);
        hash.Add(HorsePowerRpm);
        hash.Add(TorqueFtLbs);
        hash.Add(TorqueRpm);
        return hash.ToHashCode();
    }

    public static ModelEngine Create(
        VehicleModel model,
        object externalId,
        DateTimeOffset insertedAt,
        decimal? valves,
        decimal? horsePowerHp,
        decimal? horsePowerRpm,
        decimal? torqueFtLbs,
        decimal? torqueRpm)
        => Create(modelId: model.Id,
        externalId: externalId,
        insertedAt: insertedAt,
        valves: valves,
        horsePowerHp: horsePowerHp,
        horsePowerRpm: horsePowerRpm,
        torqueFtLbs: torqueFtLbs,
        torqueRpm: torqueRpm);

    public static ModelEngine Create(
        Guid modelId,
        object externalId,
        DateTimeOffset insertedAt,
        decimal? valves,
        decimal? horsePowerHp,
        decimal? horsePowerRpm,
        decimal? torqueFtLbs,
        decimal? torqueRpm)
    {
        if (valves < 0)
            throw new CommonCoreException("Invalid 'valves' number.");

        if (horsePowerHp < 0)
            throw new CommonCoreException("Invalid 'horsePowerHp' number.");

        if (horsePowerRpm < 0)
            throw new CommonCoreException("Invalid 'horsePowerRpm' number.");

        if (torqueFtLbs < 0)
            throw new CommonCoreException("Invalid 'torqueFtLbs' number.");

        if (torqueRpm < 0)
            throw new CommonCoreException("Invalid 'torqueRpm' number.");

        if (externalId is null)
            throw new CommonCoreException("Invalid 'externalId'.");

        return new ModelEngine(
            modelId: modelId,
            externalId: externalId,
            insertedAt: insertedAt,
            valves: valves,
            horsePowerHp: horsePowerHp,
            horsePowerRpm: horsePowerRpm,
            torqueFtLbs: torqueFtLbs,
            torqueRpm: torqueRpm);

    }
}
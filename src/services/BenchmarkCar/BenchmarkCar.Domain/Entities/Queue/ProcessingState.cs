using System.Text.RegularExpressions;

namespace BenchmarkCar.Domain.Entities.Queue;

public class ProcessingState
    : Entity
{
    public Guid Id { get; private set; }
    public ProcessingStateCode Code { get; private set; }
    public double Percent { get; private set; }
    public string Area { get; private set; }
    public string Key { get; private set; }

    private ProcessingState(
        Guid id, 
        ProcessingStateCode code, 
        double percent, 
        string area, 
        string key)
    {
        Id = id;
        Code = code;
        Percent = percent;
        Area = area;
        Key = key;
    }

    public override bool Equals(object? obj)
    {
        return obj is ProcessingState state &&
               base.Equals(obj) &&
               EntityId.Equals(state.EntityId) &&
               Id.Equals(state.Id) &&
               Code == state.Code &&
               Percent == state.Percent &&
               Area == state.Area &&
               Key == state.Key;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), EntityId, Id, Code, Percent, Area, Key);
    }

    public bool IsProcessFinished()
        => Code != ProcessingStateCode.Running;

    public void ChangePercentTo(
        double percent)
    {
        if (IsInvalidPercent(percent))
            throw new CommonCoreException("Invalid percent. It needs to be 0 to 100.");

        if (Code != ProcessingStateCode.Running)
            throw new CommonCoreException("");

        if (percent >= 100)
            Code = ProcessingStateCode.Processed;

        Percent = percent;
    }

    public void FinishWithStatusCode(
        ProcessingStateCode code)
    {
        if (Code == ProcessingStateCode.Running)
            throw new CommonCoreException($"Current state '{Code}' can't be changed to '{code}'.");

        if (code == Code)
            return;

        Code = code;
    }

    public static ProcessingState Create(
        Guid id,
        double percent,
        string area,
        string key)
    {
        if (!Regex.IsMatch(area, @"^[a-z0-9]{0,45}$", RegexOptions.IgnoreCase))
            throw new CommonCoreException("Invalid area.");

        if (!Regex.IsMatch(key, @"^[a-z0-9- _ ]{0,100}$", RegexOptions.IgnoreCase))
            throw new CommonCoreException("Invalid Key.");

        if (IsInvalidPercent(percent))
            throw new CommonCoreException("Invalid percent. It needs to be 0 to 100.");

        return new ProcessingState(
            id: id,
            code: ProcessingStateCode.Running,
            percent: percent,
            area: area,
            key: key);
    }

    private static bool IsInvalidPercent(double percent)
        => percent < 0.00 ||
            percent > 100.00;
}

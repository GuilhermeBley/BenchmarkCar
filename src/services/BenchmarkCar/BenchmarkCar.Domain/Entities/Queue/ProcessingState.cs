using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
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
    public IReadOnlyDictionary<string, object> MetaData { get; private set; }
    public ProcessingResult? Result { get; private set; }

    private ProcessingState(
        Guid id,
        ProcessingStateCode code,
        double percent,
        string area,
        string key,
        IReadOnlyDictionary<string, object> metaData)
    {
        Id = id;
        Code = code;
        Percent = percent;
        Area = area;
        Key = key;
        MetaData = metaData;
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

    public bool TrySetResult(
        object? resultData,
        DateTimeOffset? expirationDate = null)
    {
        if (Result is not null)
            return false;

        DateTimeOffset expirationDateToCheck =
            expirationDate ??
            DateTimeOffset.UtcNow.AddHours(1);

        if (!ProcessingResult.IsValidadExpirationDate(expirationDateToCheck))
            return false;

        try
        {
            Result = new(
                LinkedProccessId: this.Id,
                Data: resultData,
                ExpirationDate: expirationDateToCheck);
        }
        catch
        {
            return false;
        }

        return true;
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
               Key == state.Key &&
               EqualityComparer<IReadOnlyDictionary<string, object>>.Default.Equals(MetaData, state.MetaData);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(Id);
        hash.Add(Code);
        hash.Add(Percent);
        hash.Add(Area);
        hash.Add(Key);
        hash.Add(MetaData);
        return hash.ToHashCode();
    }

    public static ProcessingState Create(
        Guid id,
        double percent,
        string area,
        string key)
        => Create(
            id: id,
            percent: percent,
            area: area,
            key: key,
            metaData: ImmutableDictionary<string, object>.Empty);

    public static ProcessingState Create(
        Guid id,
        double percent,
        string area,
        string key,
        IReadOnlyDictionary<string, object> metaData)
    {
        if (!Regex.IsMatch(area, @"^[a-z0-9]{0,45}$", RegexOptions.IgnoreCase))
            throw new CommonCoreException("Invalid area.");

        if (!Regex.IsMatch(key, @"^[a-z0-9- _ ]{0,255}$", RegexOptions.IgnoreCase))
            throw new CommonCoreException("Invalid Key.");

        if (IsInvalidPercent(percent))
            throw new CommonCoreException("Invalid percent. It needs to be 0 to 100.");

        return new ProcessingState(
            id: id,
            code: ProcessingStateCode.Running,
            percent: percent,
            area: area,
            key: key,
            metaData: metaData);
    }

    private static bool IsInvalidPercent(double percent)
        => percent < 0.00 ||
            percent > 100.00;

}

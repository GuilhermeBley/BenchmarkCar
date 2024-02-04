namespace BenchmarkCar.Domain.Entities.Queue;

public enum ProcessingStateCode
    : int
{
    Running = 0,
    Processed = 1,
    Stoped = 2,
    StopedWitError = 3,
}

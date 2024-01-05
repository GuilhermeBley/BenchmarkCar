namespace BenchmarkCar.Domain.Exceptions;

public class ConflictCoreException : CoreException
{
    public const string DefaultMessage = "Conflict";
    public override int StatusCode => (int)System.Net.HttpStatusCode.Conflict;
    public ConflictCoreException(string? message = DefaultMessage) : base(message)
    {
    }
}

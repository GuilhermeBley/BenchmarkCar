namespace BenchmarkCar.Domain.Exceptions;

public class NotFoundCoreException : CoreException
{
    public const string DefaultMessage = "Not Found";
    public override int StatusCode => (int)System.Net.HttpStatusCode.NotFound;
    public NotFoundCoreException(string? message = DefaultMessage) : base(message)
    {
    }
}

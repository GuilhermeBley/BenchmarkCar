namespace BenchmarkCar.Domain.Exceptions;

public interface ICoreException
{
    int StatusCode { get; }
    string Message { get; }
}

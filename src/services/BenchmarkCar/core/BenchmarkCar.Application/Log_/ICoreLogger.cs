namespace BenchmarkCar.Application.Log;

public interface ICoreLogger<T>
    : ICoreLogger
{

}

public interface ICoreLogger
{
    void LogTrace(string message, params object[] args);
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(string message, params object[] args);
}

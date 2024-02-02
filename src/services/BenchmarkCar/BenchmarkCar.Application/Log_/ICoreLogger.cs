namespace BenchmarkCar.Application.Log;

public interface ICoreLogger<T>
    : ICoreLogger
{

}

public interface ICoreLogger
{
    void LogTrace(string? message, params object?[] args);
    void LogInformation(string? message, params object?[] args);
    void LogWarning(string? message, params object?[] args);
    void LogError(string? message, params object?[] args);
    void LogCritical(string? message, params object?[] args);
    void LogTrace(Exception e, string? message, params object?[] args);
    void LogInformation(Exception e, string? message, params object?[] args);
    void LogWarning(Exception e, string? message, params object?[] args);
    void LogError(Exception e, string? message, params object?[] args);
    void LogCritical(Exception e, string? message, params object?[] args);
}

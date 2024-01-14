using BenchmarkCar.Application.Log;
using Microsoft.Extensions.Logging;

namespace BenchmarkCar.Infrastructure.Log;

internal class InfrastructureLogger
    : ICoreLogger
{
    private readonly ILogger _logger;

    public InfrastructureLogger(ILoggerFactory factory)
    {
        _logger = factory.CreateLogger("Default");
    }

    public void LogCritical(string? message, params object?[] args)
        => _logger.LogCritical(message, args);

    public void LogError(string? message, params object?[] args)
        => _logger.LogError(message, args);

    public void LogInformation(string? message, params object?[] args)
        => _logger.LogInformation(message, args);

    public void LogTrace(string? message, params object?[] args)
        => _logger.LogTrace(message, args);

    public void LogWarning(string? message, params object?[] args)
        => _logger.LogWarning(message, args);
}

internal class InfrastructureLogger<T>
    : ICoreLogger<T>
{
    private readonly ILogger _logger;

    public InfrastructureLogger(
        ILoggerFactory factory)
    {
        _logger = factory.CreateLogger<T>();
    }

    public void LogCritical(string? message, params object?[] args)
        => _logger.LogCritical(message, args);

    public void LogError(string? message, params object?[] args)
        => _logger.LogError(message, args);

    public void LogInformation(string? message, params object?[] args)
        => _logger.LogInformation(message, args);

    public void LogTrace(string? message, params object?[] args)
        => _logger.LogTrace(message, args);

    public void LogWarning(string? message, params object?[] args)
        => _logger.LogWarning(message, args);
}

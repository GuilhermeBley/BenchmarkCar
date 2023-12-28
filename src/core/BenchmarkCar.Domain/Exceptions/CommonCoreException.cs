using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace BenchmarkCar.Domain.Exceptions;

internal class CommonCoreException
    : CoreException
{
    public const string DefaultMessage = "BadRequest";
    public override int StatusCode => (int)System.Net.HttpStatusCode.BadRequest;
    public CommonCoreException(string? message = DefaultMessage) : base(message)
    {
    }

    public static void ThrowIfNullOrWhiteSpace([NotNull] string? value, string? errorMessage = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new CommonCoreException(errorMessage ?? DefaultMessage);
    }

    public static void ThrowIfNullOrEmpty([NotNull] string? value, string? errorMessage = null)
    {
        if (string.IsNullOrEmpty(value))
            throw new CommonCoreException(errorMessage ?? DefaultMessage);
    }

    public static void ThrowIfEmpty(IEnumerable values, string? errorMessage = null)
    {
        foreach (var _ in values)
        {
            return;
        }

        throw new CommonCoreException(errorMessage ?? DefaultMessage);
    }

    public static void ThrowIfNull([NotNull] object? obj, string? errorMessage = null)
    {
        if (obj is null)
            throw new CommonCoreException(errorMessage ?? DefaultMessage);
    }

    public static void ThrowIf(bool condition, string? errorMessage = null)
    {
        if (condition)
            throw new CommonCoreException(errorMessage ?? DefaultMessage);
    }
}

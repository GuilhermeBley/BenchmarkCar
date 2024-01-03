using System.Runtime.Serialization;

namespace BenchmarkCar.Domain.Exceptions;

public abstract class CoreException
    : Exception, ICoreException
{
    /// <summary>
    /// Status code
    /// </summary>
    public abstract int StatusCode { get; }

    /// <summary>
    /// Source Core
    /// </summary>
    public override string? Source => "BenchmarkCar.Domain";

    protected CoreException()
    {
    }

    protected CoreException(string? message) : base(message)
    {
    }

    protected CoreException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    protected CoreException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public override string ToString()
    {
        return $"{StatusCode}|{base.Message}";
    }
}

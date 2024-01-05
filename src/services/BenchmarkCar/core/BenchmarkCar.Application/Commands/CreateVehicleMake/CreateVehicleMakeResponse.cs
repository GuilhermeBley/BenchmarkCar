namespace BenchmarkCar.Application.Commands.CreateVehicleMake;

public record CreateVehicleMakeResponse(
    Guid Id,
    string NormalizedName,
    string Name,
    string ExternalId,
    DateTimeOffset InsertedAt)
{
}
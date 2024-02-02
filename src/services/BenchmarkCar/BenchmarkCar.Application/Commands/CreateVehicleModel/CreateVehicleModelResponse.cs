namespace BenchmarkCar.Application.Commands.CreateVehicleModel;

public record CreateVehicleModelResponse(
    Guid Id,
    Guid VehicleMakeId,
    string Name,
    string NormalizedName,
    string? Description,
    string ExternalId ,
    DateTimeOffset InsertedAt);
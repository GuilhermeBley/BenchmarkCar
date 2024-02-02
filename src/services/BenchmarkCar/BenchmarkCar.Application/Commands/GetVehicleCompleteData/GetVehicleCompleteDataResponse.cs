using BenchmarkCar.Application.Model.Vehicles;

namespace BenchmarkCar.Application.Commands.GetVehicleCompleteData;

public record GetVehicleCompleteDataResponse(
    Guid Id,
    string Name,
    int Year,
    string NormalizedName,
    string? Description,
    string ModelExternalId,
    string EngineExternalId,
    decimal? Valves,
    decimal? HorsePowerHp,
    decimal? HorsePowerRpm,
    decimal? TorqueFtLbs,
    decimal? TorqueRpm,
    string BodyExternalId,
    int Doors,
    decimal? Length,
    decimal? Width,
    int Seats,
    decimal? EngineSize);
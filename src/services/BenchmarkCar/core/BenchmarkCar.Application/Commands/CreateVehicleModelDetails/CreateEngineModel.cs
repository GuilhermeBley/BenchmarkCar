namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

public record CreateEngineModel(
    decimal? Valves,
    decimal? HorsePowerHp,
    decimal? HorsePowerRpm,
    decimal? TorqueFtLbs,
    decimal? TorqueRpm);


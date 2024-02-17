namespace BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearch;

public record CreateEngineModel(
    string ExternalId,
    decimal? Valves,
    int? EngineSize,
    decimal? HorsePowerHp,
    decimal? HorsePowerRpm,
    decimal? TorqueFtLbs,
    decimal? TorqueRpm);


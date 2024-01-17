﻿namespace BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc;

public record CreateEngineModel(
    string ExternalId,
    decimal? Valves,
    int? EngineSize,
    decimal? HorsePowerHp,
    decimal? HorsePowerRpm,
    decimal? TorqueFtLbs,
    decimal? TorqueRpm);


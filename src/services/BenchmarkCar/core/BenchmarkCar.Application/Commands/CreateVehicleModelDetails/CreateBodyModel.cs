namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

public record CreateBodyModel(
    int Door,
    int Seats,
    decimal? Length = null,
    decimal? Width = null,
    decimal? EngineSize = null);
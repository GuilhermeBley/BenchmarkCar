namespace BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearch;

public record CreateBodyModel(
    string ExternalId,
    int Door,
    int Seats,
    decimal? Length = null,
    decimal? Width = null);
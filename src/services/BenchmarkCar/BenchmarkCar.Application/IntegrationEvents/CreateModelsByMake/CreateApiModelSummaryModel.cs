namespace BenchmarkCar.Application.IntegrationEvents.CreateModelsByMake;

public record CreateApiModelSummaryModel(
    object ExternalId,
    int Year,
    string Name,
    string Description);

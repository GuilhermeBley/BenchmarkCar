namespace BenchmarkCar.Application.Commands.GetModelsByMake;

public record GetModelsByMakeResponse(
    IQueryable<GetModelsByMakeItemResponse> Values);

public record GetModelsByMakeItemResponse(
        Guid Id,
        Guid VehicleMakeId,
        string Name,
        int Year,
        string NormalizedName,
        string? Description,
        DateTimeOffset InsertedAt);

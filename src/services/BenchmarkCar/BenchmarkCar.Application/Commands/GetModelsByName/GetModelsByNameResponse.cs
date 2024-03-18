namespace BenchmarkCar.Application.Commands.GetModelsByName;

public record GetModelsByNameResponse(
    IEnumerable<GetModelsByNameItemResponse> Items
);

public record GetModelsByNameItemResponse(
    string MakeName,
    string ModelName,
    string EntireName,
    Guid ModelId,
    int ModelYear);
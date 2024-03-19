namespace BenchmarkCar.Application.Commands.GetModelsByName;

public record GetMakesAndModelsByFilterResponse(
    IEnumerable<GetMakesAndModelsByFilterItemResponse> Items
);

public record GetMakesAndModelsByFilterItemResponse(
    string MakeName,
    string ModelName,
    string EntireName,
    Guid ModelId,
    int ModelYear);
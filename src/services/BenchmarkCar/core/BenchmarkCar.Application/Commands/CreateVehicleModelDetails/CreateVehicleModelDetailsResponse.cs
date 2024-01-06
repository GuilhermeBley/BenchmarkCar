namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

public record CreateVehicleModelDetailsResponse(
    Guid? EngineIdCreatedOrUpdated,
    Guid? BodyIdCreatedOrUpdated)
{
    public readonly static CreateVehicleModelDetailsResponse Default =
        new CreateVehicleModelDetailsResponse(null, null);
}

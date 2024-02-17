using MediatR;

namespace BenchmarkCar.Application.Commands.CreateVehicleModelDetails;

public record CreateVehicleModelDetailsRequest(
    Guid ModelId,
    CreateBodyModel? Body,
    CreateEngineModel? Engine)
    : IRequest<CreateVehicleModelDetailsResponse>;

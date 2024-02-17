using BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearch;
using MediatR;

namespace BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearch;

public record CreateVehicleModelDetailsRequest(
    Guid ModelId,
    CreateBodyModel? Body,
    CreateEngineModel? Engine)
    : IRequest<CreateVehicleModelDetailsResponse>;

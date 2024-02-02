using BenchmarkCar.Application.Repositories;
using BenchmarkCar.EventBus.Abstractions;
using BenchmarkCar.EventBus.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.CreateVehicleMakeSummary;

public class RequestVehicleModelSummaryHandler
    : IRequestHandler<RequestVehicleModelSummaryRequest, RequestVehicleModelSummaryResponse>
{
    private readonly VehicleContext _vehicleContext;
    private readonly ICoreLogger<RequestVehicleModelSummaryHandler> _logger;
    private readonly IEventBus _eventBus;

    public RequestVehicleModelSummaryHandler(
        VehicleContext vehicleContext,
        ICoreLogger<RequestVehicleModelSummaryHandler> logger,
        IEventBus eventBus)
    {
        _vehicleContext = vehicleContext;
        _logger = logger;
        _eventBus = eventBus;
    }

    public async Task<RequestVehicleModelSummaryResponse> Handle(
        RequestVehicleModelSummaryRequest request, 
        CancellationToken cancellationToken)
    {
        var makeFound = await _vehicleContext
            .VehiclesMakes
            .AsNoTracking()
            .Where(v => v.Id == request.MakeId)
            .FirstOrDefaultAsync();

        if (makeFound is null)
            throw new NotFoundCoreException("Make not found.");

        await _eventBus.PublishAsync(
            new CreateModelsByMakeIntegrationEvent()
            {
                MakeId = makeFound.Id.ToString(),
                ExternalMakeId = makeFound.ExternalId
            });

        _logger.LogInformation("Make '{0}' requested to search models.", request.MakeId);

        return new RequestVehicleModelSummaryResponse();
    }
}

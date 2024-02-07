using BenchmarkCar.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.GetVehicleCompleteData;

public class GetVehicleCompleteDataHandler
    : IRequestHandler<GetVehicleCompleteDataRequest, GetVehicleCompleteDataResponse>
{
    private readonly BenchmarkVehicleContext _vehicleContext;

    public GetVehicleCompleteDataHandler(BenchmarkVehicleContext vehicleContext)
    {
        _vehicleContext = vehicleContext;
    }

    public async Task<GetVehicleCompleteDataResponse> Handle(
        GetVehicleCompleteDataRequest request, 
        CancellationToken cancellationToken)
    {
        var vehicleData =
            await
            (from model in _vehicleContext.VehiclesModels.AsNoTracking()
             join engine in _vehicleContext.EngineModels.AsNoTracking()
                 on model.Id equals engine.ModelId
             join body in _vehicleContext.ModelBodies.AsNoTracking()
                 on model.Id equals body.ModelId
             where model.Id == body.ModelId
             select new GetVehicleCompleteDataResponse(
                model.Id,
                model.Name,
                model.Year,
                model.NormalizedName,
                model.Description,
                model.ExternalId,
                engine.ExternalId,
                engine.Valves,
                engine.HorsePowerHp,
                engine.HorsePowerRpm,
                engine.TorqueFtLbs,
                engine.TorqueRpm,
                body.ExternalId,
                body.Doors,
                body.Length,
                body.Width,
                body.Seats,
                body.EngineSize))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (vehicleData is null)
            throw new NotFoundCoreException();

        return vehicleData;
    }
}

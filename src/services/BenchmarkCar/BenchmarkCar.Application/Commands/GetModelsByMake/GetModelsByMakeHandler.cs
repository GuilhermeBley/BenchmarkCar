using BenchmarkCar.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.GetModelsByMake;

public class GetModelsByMakeHandler
    : IRequestHandler<GetModelsByMakeRequest, GetModelsByMakeResponse>
{
    private readonly ICoreLogger<GetModelsByMakeHandler> _logger;
    private readonly VehicleContext _vehicleContext;

    public GetModelsByMakeHandler(
        ICoreLogger<GetModelsByMakeHandler> logger, 
        VehicleContext vehicleContext)
    {
        _logger = logger;
        _vehicleContext = vehicleContext;
    }

    public async Task<GetModelsByMakeResponse> Handle(
        GetModelsByMakeRequest request, 
        CancellationToken cancellationToken)
    {
        var makeFound =
            await _vehicleContext
            .VehiclesMakes
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == request.MakeId);

        if (makeFound is null)
            throw new Domain.Exceptions.NotFoundCoreException("Make not found.");

        var models = _vehicleContext
            .VehiclesModels
            .AsNoTracking()
            .Where(v => v.VehicleMakeId == makeFound.Id)
            .Select(v => new GetModelsByMakeItemResponse(
                v.Id,
                v.VehicleMakeId,
                v.Name,
                v.Year,
                v.NormalizedName,
                v.Description,
                v.InsertedAt));

        return new GetModelsByMakeResponse(models);
    }
}

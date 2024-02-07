using BenchmarkCar.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.GetAllMakes;

public class GetAllMakesHandler
    : IRequestHandler<GetAllMakesRequest, IEnumerable<MakeResponse>>
{
    private readonly BenchmarkVehicleContext _vehicleContext;
    private readonly ICoreLogger<GetAllMakesHandler> _logger;

    public GetAllMakesHandler(BenchmarkVehicleContext vehicleContext, ICoreLogger<GetAllMakesHandler> logger)
    {
        _vehicleContext = vehicleContext;
        _logger = logger;
    }

    public async Task<IEnumerable<MakeResponse>> Handle(GetAllMakesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("requesting all makes.");

        return await _vehicleContext
            .VehiclesMakes
            .Select(m => new MakeResponse(m.Id, m.NormalizedName, m.Name))
            .ToListAsync(cancellationToken);
    }
}

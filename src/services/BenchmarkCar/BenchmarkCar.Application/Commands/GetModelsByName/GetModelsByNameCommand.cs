using BenchmarkCar.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.GetModelsByName;

public class GetModelsByNameCommand
    : IRequestHandler<GetModelsByNameRequest, GetModelsByNameResponse>
{
    private readonly BenchmarkVehicleContext _context;
    private readonly ICoreLogger _logger;

    public GetModelsByNameCommand(
        BenchmarkVehicleContext context,
        ICoreLogger<GetModelsByNameCommand> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetModelsByNameResponse> Handle(
        GetModelsByNameRequest request, 
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedFilter = request.Filter?.ToUpperInvariant();

        var query = 
            from make in _context.VehiclesMakes.AsNoTracking()
            join model in _context.VehiclesModels.AsNoTracking()
                on make.Id equals model.VehicleMakeId
            where 
                EF.Functions.Like(make.NormalizedName, $"%{normalizedFilter}%") || 
                EF.Functions.Like(model.NormalizedName, $"%{normalizedFilter}%")
            select new GetModelsByNameItemResponse(
                make.Name,
                model.Name,
                string.Concat(make.Name, '-', model.Name, ' ', model.Year),
                model.Id,
                model.Year
            );

        // Doing pagination and ordering
        query = query.OrderBy(e => e.EntireName).Take(100);

        return new (await query.ToListAsync(cancellationToken));
    }
}
using BenchmarkCar.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.GetModelsByName;

public class GetMakesAndModelsByFilterCommand
    : IRequestHandler<GetMakesAndModelsByFilterRequest, GetMakesAndModelsByFilterResponse>
{
    private readonly BenchmarkVehicleContext _context;
    private readonly ICoreLogger _logger;

    public GetMakesAndModelsByFilterCommand(
        BenchmarkVehicleContext context,
        ICoreLogger<GetMakesAndModelsByFilterCommand> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GetMakesAndModelsByFilterResponse> Handle(
        GetMakesAndModelsByFilterRequest request, 
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedFilter = request.Filter?.Trim('\n', ' ', '\t').ToUpperInvariant() ?? string.Empty;

        var queryJoin =
            from make in _context.VehiclesMakes.AsNoTracking()
            join model in _context.VehiclesModels.AsNoTracking()
                on make.Id equals model.VehicleMakeId
            select new { make, model };
            
        if (normalizedFilter.Length > 1)
        {
            queryJoin =
            from make in _context.VehiclesMakes.AsNoTracking()
            join model in _context.VehiclesModels.AsNoTracking()
                on make.Id equals model.VehicleMakeId
            where
                make.NormalizedName.Contains($"{normalizedFilter}") ||
                model.NormalizedName.Contains($"{normalizedFilter}")
            select new { make, model };
        }
        
        var query =
            (from item in queryJoin
             orderby item.make.NormalizedName
            select new GetMakesAndModelsByFilterItemResponse(
                item.make.Name,
                item.model.Name,
                string.Concat(item.make.Name, '-', item.model.Name, ' ', item.model.Year),
                item.model.Id,
                item.model.Year
            ))
            .Take(100);

        return new (await query.ToListAsync(cancellationToken));
    }
}
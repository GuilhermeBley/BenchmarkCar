using BenchmarkCar.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.GetProcessingStateById;

public class GetProcessingStateByIdHandler
    : IRequestHandler<GetProcessingStateByIdRequest, GetProcessingStateByIdResponse>
{
    private readonly ICoreLogger<GetProcessingStateByIdHandler> _logger;
    private readonly BenchmarkVehicleContext _context;

    public GetProcessingStateByIdHandler(
        ICoreLogger<GetProcessingStateByIdHandler> logger, 
        BenchmarkVehicleContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<GetProcessingStateByIdResponse> Handle(
        GetProcessingStateByIdRequest request, 
        CancellationToken cancellationToken)
    {
        var processingFound = 
            await _context
                .ProcessingQueues
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.ProcessId, cancellationToken);

        _logger.LogInformation("Processing '{0}' wasn't found.", request.ProcessId);

        if (processingFound is null)
            throw new NotFoundCoreException("Proccess not found.");

        _logger.LogInformation("Processing '{0}' sucessfully found.", processingFound.Id);

        return new GetProcessingStateByIdResponse(
            processingFound.Id,
            processingFound.Code,
            processingFound.Percent);
    }
}

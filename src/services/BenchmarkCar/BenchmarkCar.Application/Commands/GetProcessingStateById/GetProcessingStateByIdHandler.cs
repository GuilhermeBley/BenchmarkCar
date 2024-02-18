using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Queue;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
        
        object? result = null;
        if (processingFound.Code == (int)ProcessingStateCode.Processed)
        {
            var jsonText = (await _context
                .ProcessingResults
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.LinkedProccessId == processingFound.Id, cancellationToken))?
                .Data;
            if (!string.IsNullOrWhiteSpace(jsonText))
                result = JsonSerializer.Deserialize<dynamic>(jsonText);
        }

        _logger.LogInformation("Processing '{0}' sucessfully found.", processingFound.Id);

        return new GetProcessingStateByIdResponse(
            processingFound.Id,
            processingFound.Code,
            processingFound.Percent,
            Result: result);
    }
}

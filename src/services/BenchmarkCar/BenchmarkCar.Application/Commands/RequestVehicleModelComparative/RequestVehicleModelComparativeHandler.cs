﻿using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Domain.Entities.Queue;
using BenchmarkCar.EventBus.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Commands.RequestVehicleModelComparative;

public class RequestVehicleModelComparativeHandler
    : IRequestHandler<RequestVehicleModelComparativeRequest, RequestVehicleModelComparativeResponse>
{
    private readonly IEventBus _eventBus;
    private readonly ICoreLogger<RequestVehicleModelComparativeHandler> _logger;
    private readonly BenchmarkVehicleContext _context;

    public RequestVehicleModelComparativeHandler(
        IEventBus eventBus, 
        ICoreLogger<RequestVehicleModelComparativeHandler> logger, 
        BenchmarkVehicleContext context)
    {
        _eventBus = eventBus;
        _logger = logger;
        _context = context;
    }

    public async Task<RequestVehicleModelComparativeResponse> Handle(
        RequestVehicleModelComparativeRequest request, 
        CancellationToken cancellationToken)
    {
        var modelX = await _context
            .VehiclesModels
            .FirstOrDefaultAsync(x => x.Id == request.modelIdX);

        if (modelX is null)
            throw new NotFoundCoreException("Vehicle not found.");

        var modelY = await _context
            .VehiclesModels
            .FirstOrDefaultAsync(x => x.Id == request.modelIdY);

        if (modelY is null)
            throw new NotFoundCoreException("Vehicle not found.");

        var entity = ProcessingState.Create(
            Guid.NewGuid(),
            0.00,
            nameof(RequestVehicleModelComparativeHandler),
            string.Concat(
                modelX.Id,
                '-',
                modelY.Id));

        using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);

        var resultModel =
            await _context
                .ProcessingQueues
                .AddAsync(
                    Model.Queue.ProcessingStateModel.MapFrom(entity));

        await _eventBus.PublishAsync(new EventBus.Events.RequestComparativeModelIntegrationEvent()
        {
            ProccessId = resultModel.Entity.Id,
            ModelIdY = modelY.Id,
            ModelIdX = modelX.Id,
        });

        await transaction.CommitAsync(cancellationToken);

        await _context.SaveChangesAsync();

        return new RequestVehicleModelComparativeResponse(
            ComparativeProcessingStateId: resultModel.Entity.Id);
    }
}

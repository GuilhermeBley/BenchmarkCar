using BenchmarkCar.Infrastructure.Extensions;
using BenchmarkCar.EventBus.Azure.Extensions.Di;
using Microsoft.Extensions.DependencyInjection;
using BenchmarkCar.EventBus.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(
    builder.Configuration,
    migrationAssembly: typeof(BenchmarkCar.Ui.Api.Controllers.VehicleMakeController).Assembly);

builder.Services.AddEventBus(
    subscriptionName: builder.Configuration.GetValue<string>("AzureServiceBus:BenchmarkCarSub"),
    (provider, eventBus) =>
    {
        eventBus.Subscribe<BenchmarkCar.EventBus.Events.CreateModelsByMakeIntegrationEvent,
            BenchmarkCar.Application.IntegrationEvents.CreateModelsByMake.CreateModelsByMakeHandler>();
        eventBus.Subscribe<BenchmarkCar.EventBus.Events.CreateModelIntegrationEvent,
            BenchmarkCar.Application.IntegrationEvents.ModelRequestedToSearc.ModelRequestedToSearchHandler>();
        eventBus.Subscribe<BenchmarkCar.EventBus.Events.CreateMakesIntegrationEvent,
            BenchmarkCar.Application.IntegrationEvents.MakesRequestedToCreate.MakesRequestedToCreateHandler>();
    });

builder.Services.AddOptions<BenchmarkCar.Infrastructure.Options.CarApiOptions>()
    .Bind(builder.Configuration.GetSection(BenchmarkCar.Infrastructure.Options.CarApiOptions.SECTION))
    .ValidateDataAnnotations();
builder.Services.AddOptions<BenchmarkCar.Infrastructure.Options.SqlOptions>()
    .Bind(builder.Configuration.GetSection(BenchmarkCar.Infrastructure.Options.SqlOptions.SECTION))
    .ValidateDataAnnotations();
builder.Services.AddOptions<BenchmarkCar.EventBus.Azure.AzureServiceBusOptions>()
    .Bind(builder.Configuration.GetSection(BenchmarkCar.EventBus.Azure.AzureServiceBusOptions.SECTION))
    .ValidateDataAnnotations();

var app = builder.Build();

app.Services.GetRequiredService<IEventBus>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using BenchmarkCar.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(
    builder.Configuration,
    migrationAssembly: typeof(Program).GetType().Assembly);

builder.Services.AddOptions<BenchmarkCar.Infrastructure.Options.CarApiOptions>()
    .Bind(builder.Configuration.GetSection(BenchmarkCar.Infrastructure.Options.CarApiOptions.SECTION))
    .ValidateDataAnnotations();
builder.Services.AddOptions<BenchmarkCar.Infrastructure.Options.SqlOptions>()
    .Bind(builder.Configuration.GetSection(BenchmarkCar.Infrastructure.Options.SqlOptions.SECTION))
    .ValidateDataAnnotations();

var app = builder.Build();

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

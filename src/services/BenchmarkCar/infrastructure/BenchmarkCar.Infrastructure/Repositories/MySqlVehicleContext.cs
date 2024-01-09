using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BenchmarkCar.Infrastructure.Repositories;

internal class MySqlVehicleContext
    : VehicleContext
{
    private readonly ILogger<MySqlVehicleContext> _logger;
    private IOptions<MysqlOptions> _options;

    public override DbSet<VehicleMakeModel> VehiclesMakes { get; set; } = null!;

    public override DbSet<VehicleModelModel> VehiclesModels { get; set; } = null!;

    public override DbSet<BestModelModel> BestModels { get; set; } = null!;

    public override DbSet<ModelBodyModel> ModelBodies { get; set; } = null!;

    public override DbSet<ModelEngineModel> EngineModels { get; set; } = null!;

    public MySqlVehicleContext(
        ILogger<MySqlVehicleContext> logger,
        IOptions<Options.MysqlOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseMySql(
            _options.Value.ConnectionString,
            serverVersion: ServerVersion.AutoDetect(_options.Value.ConnectionString),
            config =>
            {
            });
    }
}

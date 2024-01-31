using BenchmarkCar.Application.Model.Vehicles;
using BenchmarkCar.Application.Repositories;
using BenchmarkCar.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BenchmarkCar.Infrastructure.Repositories;

public class SqlVehicleContext
    : VehicleContext
{
    private readonly ILogger<SqlVehicleContext> _logger;
    private IOptions<SqlOptions> _options;

    public override DbSet<VehicleMakeModel> VehiclesMakes { get; set; } = null!;

    public override DbSet<VehicleModelModel> VehiclesModels { get; set; } = null!;

    public override DbSet<BestModelModel> BestModels { get; set; } = null!;

    public override DbSet<ModelBodyModel> ModelBodies { get; set; } = null!;

    public override DbSet<ModelEngineModel> EngineModels { get; set; } = null!;

    public SqlVehicleContext(
        ILogger<SqlVehicleContext> logger,
        IOptions<Options.SqlOptions> options,
        DbContextOptions<SqlVehicleContext> dbOptions)
        : base(dbOptions)
    {
        _logger = logger;
        _options = options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VehicleMakeModel>(cfg =>
        {
            cfg.HasKey(p => p.Id);
            cfg.HasIndex(p => p.NormalizedName).IsUnique();
            cfg.Property(e => e.NormalizedName)
                .HasColumnType("varchar(255)");
            cfg.Property(e => e.Name)
                .HasColumnType("varchar(255)");
            cfg.Property(e => e.ExternalId)
                .HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<VehicleModelModel>(cfg =>
        {
            cfg.HasKey(p => p.Id);
            cfg.HasIndex(p => p.NormalizedName).IsUnique();
            cfg.HasOne(p => p.VehicleMake).WithMany().HasForeignKey(p => p.VehicleMakeId);
            cfg.Property(e => e.NormalizedName)
                .HasColumnType("varchar(255)");
            cfg.Property(e => e.Name)
                .HasColumnType("varchar(255)");
            cfg.Property(e => e.ExternalId)
                .HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<BestModelModel>(cfg =>
        {
            cfg.HasKey(p => p.Id);
            cfg.HasOne<VehicleModelModel>().WithMany().HasForeignKey(p => p.VehicleModelId);
            cfg.Property(e => e.Area)
                .HasColumnType("varchar(255)");
        });

        modelBuilder.Entity<ModelBodyModel>(cfg =>
        {
            cfg.HasKey(p => p.ModelId);
            cfg.HasIndex(p => p.ExternalId).IsUnique();
            cfg.HasOne<VehicleModelModel>().WithMany().HasForeignKey(p => p.ModelId);
            cfg.Property(e => e.ExternalId)
                .HasColumnType("varchar(255)");
            cfg.Property(e => e.Length)
                .HasColumnType("decimal(10,2)");
            cfg.Property(e => e.EngineSize)
                .HasColumnType("decimal(10,2)");
            cfg.Property(e => e.Width)
                .HasColumnType("decimal(10,2)");
        });

        modelBuilder.Entity<ModelEngineModel>(cfg =>
        {
            cfg.HasKey(p => p.ModelId);
            cfg.HasIndex(p => p.ExternalId).IsUnique();
            cfg.HasOne<VehicleModelModel>().WithMany().HasForeignKey(p => p.ModelId);
            cfg.Property(e => e.ExternalId)
                .HasColumnType("varchar(255)");
            cfg.Property(e => e.TorqueRpm)
                .HasColumnType("decimal(10,2)");
            cfg.Property(e => e.TorqueFtLbs)
                .HasColumnType("decimal(10,2)");
            cfg.Property(e => e.Valves)
                .HasColumnType("decimal(10,2)");
            cfg.Property(e => e.HorsePowerHp)
                .HasColumnType("decimal(10,2)");
            cfg.Property(e => e.HorsePowerRpm)
                .HasColumnType("decimal(10,2)");
        });
    }
}

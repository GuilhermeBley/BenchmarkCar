using BenchmarkCar.Application.Model.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Repositories;

public abstract class VehicleContext
    : DbContext
{
    public abstract DbSet<VehicleMakeModel> VehiclesMakes { get; }
    public abstract DbSet<VehicleModelModel> VehiclesModels { get; }
    public abstract DbSet<BestModelModel> BestModels { get; }
    public abstract DbSet<ModelBodyModel> ModelBodies { get; }
    public abstract DbSet<ModelEngineModel> EngineModels { get; }
}

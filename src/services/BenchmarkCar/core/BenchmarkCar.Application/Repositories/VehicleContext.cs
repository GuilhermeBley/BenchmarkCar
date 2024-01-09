using BenchmarkCar.Application.Model.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Repositories;

public abstract class VehicleContext
    : DbContext
{
    public abstract DbSet<VehicleMakeModel> VehiclesMakes { get; set; }
    public abstract DbSet<VehicleModelModel> VehiclesModels { get; set; }
    public abstract DbSet<BestModelModel> BestModels { get; set; }
    public abstract DbSet<ModelBodyModel> ModelBodies { get; set; }
    public abstract DbSet<ModelEngineModel> EngineModels { get; set; }
}

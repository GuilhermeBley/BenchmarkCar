using BenchmarkCar.Application.Model.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkCar.Application.Repositories;

public interface IVehicleContext
{
    DbSet<VehicleMakeModel> VehiclesMakes { get; }
    DbSet<VehicleModelModel> VehiclesModels { get; }
    DbSet<BestModelModel> BestModels { get; }
    DbSet<ModelBodyModel> ModelBodies { get; }
    DbSet<ModelEngineModel> EngineModels { get; }
}

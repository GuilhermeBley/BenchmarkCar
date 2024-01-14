using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BenchmarkCar.Infrastructure.Extensions;

public static class ApplicationServicesDiExtension
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddLogging();

        serviceCollection.AddDbContextPool<Repositories.SqlVehicleContext>(opt =>
        {
        });

        serviceCollection.AddScoped<Application.Repositories.VehicleContext, Repositories.SqlVehicleContext>();

        serviceCollection.AddSingleton<Application.Log.ICoreLogger, Log.InfrastructureLogger>();
        serviceCollection.AddSingleton(typeof(Application.Log.ICoreLogger<>), typeof(Log.InfrastructureLogger<>));

        serviceCollection.AddSingleton<Application.ExternalApi.IVehiclesDataQuery, ExternalApi.CarApiVehiclesDataQuery>();

        return serviceCollection;
    }
}

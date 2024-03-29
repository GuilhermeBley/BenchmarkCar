﻿using BenchmarkCar.Infrastructure.ExternalApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BenchmarkCar.Infrastructure.Extensions;

public static class ApplicationServicesDiExtension
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        Assembly? migrationAssembly = null)
    {
        serviceCollection.AddLogging();

        serviceCollection.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(Application.Commands.CreateVehicleMake.CreateVehicleMakeHandler).Assembly);
        });

        serviceCollection.AddDbContext<Repositories.SqlVehicleContext>((provider, opt) =>
        {
            opt.UseSqlServer(
                connectionString: provider
                    .GetRequiredService<IOptions<Options.SqlOptions>>().Value.ConnectionString,
                opt =>
                {
                    opt.MigrationsAssembly(
                        migrationAssembly?.FullName ?? typeof(ApplicationServicesDiExtension).Assembly.FullName);
                    opt.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: System.TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
        });

        serviceCollection.AddScoped<Application.Repositories.BenchmarkVehicleContext, Repositories.SqlVehicleContext>();

        serviceCollection.AddSingleton<Application.Log.ICoreLogger, Log.InfrastructureLogger>();
        serviceCollection.AddSingleton(typeof(Application.Log.ICoreLogger<>), typeof(Log.InfrastructureLogger<>));

        serviceCollection.AddHttpClient(nameof(CarApiVehiclesDataQuery), ExternalApi.CarApiVehiclesDataQuery.Configure);

        serviceCollection.AddSingleton<Application.ExternalApi.IVehiclesDataQuery, ExternalApi.CarApiVehiclesDataQuery>();

        return serviceCollection;
    }
}

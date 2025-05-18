using EvacutionPlanningAndMonitoring.App.API.Repositories;

namespace EvacutionPlanningAndMonitoring.App.API.Extensions;

public static class RepositoryDIExtenstion
{
    public static IServiceCollection AddRepositoriesDIExtension(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository,VehicleRepository>();
        services.AddScoped<IEvacutionPlanRepository,EvacutionPlanRepository>();
        services.AddScoped<IEvacutionStatusRepository,EvacutionStatusRepository>();
        services.AddScoped<IEvacutionZoneRepository,EvacutionZoneRepository>();
        return services;
    }
}

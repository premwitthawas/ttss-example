using EvacutionPlanningAndMonitoring.App.API.Helpers;
using EvacutionPlanningAndMonitoring.App.API.Services;

namespace EvacutionPlanningAndMonitoring.App.API.Extensions;

public static class ServiceDIExtension
{
    public static IServiceCollection AddServiceDIExtension(this IServiceCollection services)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IEvacutionPlanService, EvacutionPlanService>();
        services.AddScoped<IEvacutionZoneService, EvacutionZoneService>();
        services.AddScoped<IEvavacutionStatusService, EvavacutionStatusService>();
        services.AddScoped<ICalculateDistanceHelper, CalculateDistanceHelper>();
        return services;
    }
}
using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Repositories;


public interface IVehicleRepository
{
    Task<Vehicle> InsertVehicleAsync(Vehicle vehicle);
    Task<Vehicle?> SelectVehicleByIdAsync(string id);
    Task<IEnumerable<Vehicle>> SelectVehiclesAsync();
};
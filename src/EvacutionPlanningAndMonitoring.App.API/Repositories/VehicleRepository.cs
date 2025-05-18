using EvacutionPlanningAndMonitoring.App.API.Data;
using EvacutionPlanningAndMonitoring.App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EvacutionPlanningAndMonitoring.App.API.Repositories;

public class VehicleRepository(ApplicationDbContext context) : IVehicleRepository
{
    public async Task<Vehicle> InsertVehicleAsync(Vehicle vehicle)
    {
        context.Vehicle.Add(vehicle);
        await context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<Vehicle?> SelectVehicleByIdAsync(string id)
    {
        return await context.Vehicle.SingleOrDefaultAsync(x => x.VehicleID == id);
    }

    public async Task<IEnumerable<Vehicle>> SelectVehiclesAsync()
    {
        return await context.Vehicle.ToListAsync();
    }
};
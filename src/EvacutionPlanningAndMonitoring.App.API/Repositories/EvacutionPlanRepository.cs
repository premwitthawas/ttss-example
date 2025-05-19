using EvacutionPlanningAndMonitoring.App.API.Data;
using EvacutionPlanningAndMonitoring.App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EvacutionPlanningAndMonitoring.App.API.Repositories;

public class EvacutionPlanRepository(ApplicationDbContext context) : IEvacutionPlanRepository
{
    public async Task<EvacutionPlan> InsertEvacutionPlanAsync(EvacutionPlan evacutionPlan)
    {
        context.Add(evacutionPlan);
        await context.SaveChangesAsync();
        return evacutionPlan;
    }

    public async Task<IEnumerable<EvacutionPlan>> SelectAllEvacutionPlanAsync()
    {
        return await context.EvacutionPlans.Include(x => x.EvacutionZone).ToListAsync();
    }

    public async Task<EvacutionPlan?> SelectEvacutionPlanByIdAsync(string zoneId, string vehicleId)
    {
        return await context.EvacutionPlans
        .SingleOrDefaultAsync(x => x.ZoneID == zoneId && x.VehicleID == vehicleId);
    }

    public async Task<EvacutionPlan> UpdateEvacutionPlanAsync(EvacutionPlan evacutionPlan)
    {
        EvacutionPlan? evacutionPlanResult = await context.EvacutionPlans.SingleOrDefaultAsync(x => x.ZoneID == evacutionPlan.ZoneID
        && x.VehicleID == evacutionPlan.VehicleID) ?? throw new KeyNotFoundException("EvacutionPlan Not Found");
        evacutionPlanResult.UpdatedAt = DateTime.UtcNow;
        evacutionPlanResult.Vehicle ??= evacutionPlan.Vehicle;
        evacutionPlanResult.EvacutionZone ??= evacutionPlan.EvacutionZone;
        evacutionPlanResult.NumberOfPeople = evacutionPlan.NumberOfPeople;
        evacutionPlanResult.ETA ??= evacutionPlan.ETA;
        await context.SaveChangesAsync();
        return evacutionPlanResult;
    }
}
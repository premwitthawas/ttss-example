using EvacutionPlanningAndMonitoring.App.API.Data;
using EvacutionPlanningAndMonitoring.App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EvacutionPlanningAndMonitoring.App.API.Repositories;

public class EvacutionZoneRepository(ApplicationDbContext context) : IEvacutionZoneRepository
{
    public async Task<EvacutionZone> InsertEvacutionZoneAsync(EvacutionZone evacutionZone)
    {
        context.Add(evacutionZone);
        await context.SaveChangesAsync();
        return evacutionZone;
    }

    public async Task<EvacutionZone?> SelectEvacutionZoneByIdAsync(string id)
    {
        return await context.EvacutionZones.SingleOrDefaultAsync(x => x.ZoneID == id);
    }

    public async Task<IEnumerable<EvacutionZone>> SelectEvacutionZonesAsync()
    {
        return await context.EvacutionZones.ToListAsync();
    }
}
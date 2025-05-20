using EvacutionPlanningAndMonitoring.App.API.Data;
using EvacutionPlanningAndMonitoring.App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EvacutionPlanningAndMonitoring.App.API.Repositories;

public class EvacutionStatusRepository(ApplicationDbContext context) : IEvacutionStatusRepository
{
    public async Task<IEnumerable<EvacutionStatus>> SelectAllEvacutionDefualtStatusAsync()
    {
        // throw new NotImplementedException();
        return await context.EvacutionStatuses.Include(x=>x.EvacutionZone).OrderByDescending(x => x.EvacutionZone!.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<EvacutionStatus>> SelectAllEvacutionStatusAsync(int skip, int take, string? keyword)
    {
        var query = context.EvacutionStatuses.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ZoneID.Contains(keyword.Trim()) || x.LastVechicleUsed.Contains(keyword.Trim()));
        }
        return await query
            .OrderByDescending(x => x.EvacutionZone!.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> SelectAllIdsEvacutionDStatusAsync()
    {
        return await context.EvacutionStatuses.Select(x => x.ZoneID).AsNoTracking().ToListAsync();
    }

    public async Task<EvacutionStatus?> SelectEvacutionStatusByIdAsync(string id)
    {
        return await context.EvacutionStatuses.SingleOrDefaultAsync(x => x.ZoneID == id);
    }

    public async Task<EvacutionStatus> UpdateEvacutionStatusAsync(string zoneId, EvacutionStatus evacutionStatus)
    {

        await using var tx = await context.Database.BeginTransactionAsync();
        try
        {
            var exinstingStatus = await context.EvacutionStatuses.SingleOrDefaultAsync(x => x.ZoneID == zoneId) ?? throw new KeyNotFoundException("EvacutionStatus Not found with the Id");
            exinstingStatus.RemainingPeople = evacutionStatus.RemainingPeople ?? exinstingStatus.RemainingPeople;
            exinstingStatus.TotalEvacuated = evacutionStatus.TotalEvacuated ?? exinstingStatus.TotalEvacuated;
            exinstingStatus.LastVechicleUsed = evacutionStatus.LastVechicleUsed ?? exinstingStatus.LastVechicleUsed;
            await context.SaveChangesAsync();
            await tx.CommitAsync();
            return exinstingStatus;
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }
}
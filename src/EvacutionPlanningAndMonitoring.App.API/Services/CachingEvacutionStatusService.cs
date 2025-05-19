using System.Text.Json;
using EvacutionPlanningAndMonitoring.App.API.DTOs;
using Microsoft.Extensions.Caching.Distributed;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public class CachingEvacutionStatusService(IDistributedCache cache) : ICachingEvacutionStatusService
{
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);
    private readonly string _vacutionStatusesCacheKey = "evacution_status";

    public async Task<List<EvacutionStatusDTO>> GetEvacutionStatusesCaching()
    {
        string? stringStatus = await cache.GetStringAsync(_vacutionStatusesCacheKey);
        if (string.IsNullOrEmpty(stringStatus))
        {
            return [];
        }
        var status = JsonSerializer.Deserialize<List<EvacutionStatusDTO>>(stringStatus);
        if (status == null)
        {
            return [];
        }
        return status;
    }

    public async Task RemoveEvacutionStatusesCaching()
    {
        await cache.RemoveAsync(_vacutionStatusesCacheKey);
    }

    public async Task SetEvacutionStatusesCaching(List<EvacutionStatusDTO> evacutionStatusDTOs)
    {
        await cache.SetStringAsync(_vacutionStatusesCacheKey, JsonSerializer.Serialize(evacutionStatusDTOs),new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheExpiry
        });
    }
}
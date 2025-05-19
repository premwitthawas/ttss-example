using System.Text.Json;
using EvacutionPlanningAndMonitoring.App.API.DTOs;
using Microsoft.Extensions.Caching.Distributed;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public class CachingEvacutionStatusService(IDistributedCache cache) : ICachingEvacutionStatusService
{
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);
    private readonly string _vacutionStatusesCacheKey = "evacution_status";

    public async Task<ResponseEvacutionStatusDTO?> GetEvacutionStatusByZoneIdCaching(string zoneID)
    {
        string? zoneStatusString = await cache.GetStringAsync(_vacutionStatusesCacheKey + "_" + zoneID);
        if (zoneStatusString == null)
        {
            return null;
        }
        var data = JsonSerializer.Deserialize<ResponseEvacutionStatusDTO>(zoneStatusString);
        return data;
    }

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

    public async Task RemoveEvacutionStatuseByZoneIdCaching(string zondId)
    {
        await cache.RemoveAsync(_vacutionStatusesCacheKey + "_" +zondId);
    }

    public async Task RemoveEvacutionStatusesCaching()
    {
        await cache.RemoveAsync(_vacutionStatusesCacheKey);
    }

    public async Task SetEvacutionStatusByZoneId(ResponseEvacutionStatusDTO responseEvacutionStatusDTO)
    {
        await cache.SetStringAsync(_vacutionStatusesCacheKey + "_" + responseEvacutionStatusDTO.ZoneID,
        JsonSerializer.Serialize(responseEvacutionStatusDTO));
    }

    public async Task SetEvacutionStatusesCaching(List<EvacutionStatusDTO> evacutionStatusDTOs)
    {
        await cache.SetStringAsync(_vacutionStatusesCacheKey, JsonSerializer.Serialize(evacutionStatusDTOs), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheExpiry
        });
    }
}
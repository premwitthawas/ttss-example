using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;
using EvacutionPlanningAndMonitoring.App.API.Repositories;
using Serilog;


namespace EvacutionPlanningAndMonitoring.App.API.Services;

public class EvavacutionStatusService(IEvacutionStatusRepository evacutionStatusRepository, ICachingEvacutionStatusService cachingEvacutionStatusService) :
IEvavacutionStatusService
{
    public async Task<ResponseDTO<List<ResponseEvacutionStatusDTO>>> GetEvacutionDefaultStatusesAsync()
    {
        var results = await evacutionStatusRepository.SelectAllIdsEvacutionDStatusAsync();
        List<ResponseEvacutionStatusDTO> lists = [];
        foreach (var id in results.ToList())
        {
            ResponseEvacutionStatusDTO? cacheHit = await cachingEvacutionStatusService.GetEvacutionStatusByZoneIdCaching(id);
            if (cacheHit == null)
            {
                var data = await evacutionStatusRepository.SelectEvacutionStatusByIdAsync(id);
                var mapObject = new ResponseEvacutionStatusDTO(data!.ZoneID, data.RemainingPeople, data.Operations, data.IsCompleted, data.LastVechicleUsed, data.TotalEvacuated);
                lists.Add(mapObject);
                await cachingEvacutionStatusService.SetEvacutionStatusByZoneId(mapObject);
            }
            else
            {
                lists.Add(cacheHit);
            }
        }
        return new ResponseDTO<List<ResponseEvacutionStatusDTO>>(false, 200, lists, null);
    }
    public async Task<ResponseDTO<EvacutionStatusDTO>> UpdateEvacutionStatusServiceAsync(EvacutionStatusDTO evacutionStatusDTO)
    {
        Log.Information($"Update Evacution Status ZoneId: {evacutionStatusDTO.ZoneID} | RemainingPeople : {evacutionStatusDTO.RemaininPeople} | Operations : {evacutionStatusDTO.Operations} | IsCompleted : {evacutionStatusDTO.IsCompleted}");
        var mapObject = new EvacutionStatus
        {
            ZoneID = evacutionStatusDTO.ZoneID,
            LastVechicleUsed = evacutionStatusDTO.LastVechicleUsed ?? string.Empty,
            RemainingPeople = evacutionStatusDTO.RemaininPeople,
            TotalEvacuated = evacutionStatusDTO.TotalEvacuated,
            IsCompleted = evacutionStatusDTO.IsCompleted,
            Operations = evacutionStatusDTO.Operations ?? "Waiting",
        };

        var result = await evacutionStatusRepository.UpdateEvacutionStatusAsync(evacutionStatusDTO.ZoneID, mapObject);
        if (result == null)
        {
            return new ResponseDTO<EvacutionStatusDTO>(true, 404, null, "EvacutionStatus Not Found.");
        }
        var cacheHit = await cachingEvacutionStatusService.GetEvacutionStatusByZoneIdCaching(result.ZoneID);
        if (cacheHit != null)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatuseByZoneIdCaching(result.ZoneID);
            var data = new ResponseEvacutionStatusDTO(result.ZoneID, result.RemainingPeople, result.Operations, result.IsCompleted, result.LastVechicleUsed, result.TotalEvacuated);
            await cachingEvacutionStatusService.SetEvacutionStatusByZoneId(data);
        }
        var mapResult = new EvacutionStatusDTO(result.ZoneID, result.RemainingPeople, result.Operations, result.IsCompleted, result.LastVechicleUsed, result.TotalEvacuated);
        return new ResponseDTO<EvacutionStatusDTO>(false, 200, mapResult, null);
    }

    public async Task<bool> UpdateIsCompleteAsync(string zoneId, bool status)
    {
        Log.Information($"Update Evacution Status ZoneId: {zoneId} | IsCompleted : {status}");
        var statusExist = await evacutionStatusRepository.SelectEvacutionStatusByIdAsync(zoneId);
        if (statusExist == null)
        {
            return false;
        }
        statusExist.IsCompleted = status;
        var result = await evacutionStatusRepository.UpdateEvacutionStatusAsync(statusExist.ZoneID, statusExist);
        var cacheHit = await cachingEvacutionStatusService.GetEvacutionStatusByZoneIdCaching(result.ZoneID);
        if (cacheHit != null)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatuseByZoneIdCaching(result.ZoneID);
            var data = new ResponseEvacutionStatusDTO(result.ZoneID, result.RemainingPeople, result.Operations, result.IsCompleted, result.LastVechicleUsed, result.TotalEvacuated);
            await cachingEvacutionStatusService.SetEvacutionStatusByZoneId(data);
        }
        return true;
    }

    public async Task<bool> UpdateIsOperationsCompleteAsync(string zoneId)
    {
        Log.Information($"Update Evacution Status ZoneId: {zoneId} | Operations : Completed");
        var statusExist = await evacutionStatusRepository.SelectEvacutionStatusByIdAsync(zoneId);
        if (statusExist == null)
        {
            return false;
        }
        statusExist.Operations = "Completed";
        var result = await evacutionStatusRepository.UpdateEvacutionStatusAsync(statusExist.ZoneID, statusExist);
        var cacheHit = await cachingEvacutionStatusService.GetEvacutionStatusByZoneIdCaching(result.ZoneID);
        if (cacheHit != null)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatuseByZoneIdCaching(result.ZoneID);
            var data = new ResponseEvacutionStatusDTO(result.ZoneID, result.RemainingPeople, result.Operations, result.IsCompleted, result.LastVechicleUsed, result.TotalEvacuated);
            await cachingEvacutionStatusService.SetEvacutionStatusByZoneId(data);
        }
        return true;
    }

    public async Task<bool> UpdateIsOperationsWattingsAsync(string zoneId)
    {
        Log.Information($"Update Evacution Status ZoneId: {zoneId} | Operations : Waiting");
        var statusExist = await evacutionStatusRepository.SelectEvacutionStatusByIdAsync(zoneId);
        if (statusExist == null)
        {
            return false;
        }
        statusExist.Operations = "Waiting";
        var result = await evacutionStatusRepository.UpdateEvacutionStatusAsync(statusExist.ZoneID, statusExist);
        var cacheHit = await cachingEvacutionStatusService.GetEvacutionStatusByZoneIdCaching(result.ZoneID);
        System.Console.WriteLine(result.Operations);
        if (cacheHit != null)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatuseByZoneIdCaching(result.ZoneID);
            var data = new ResponseEvacutionStatusDTO(result.ZoneID, result.RemainingPeople, result.Operations, result.IsCompleted, result.LastVechicleUsed, result.TotalEvacuated);
            await cachingEvacutionStatusService.SetEvacutionStatusByZoneId(data);
        }
        return true;
    }

    public async Task<bool> UpdateRemainingPeopleAsync(string zoneId, int? people, string vehicle)
    {
        var statusExist = await evacutionStatusRepository.SelectEvacutionStatusByIdAsync(zoneId);
        if (statusExist == null)
        {
            return false;
        }
        if (statusExist.RemainingPeople < people)
        {
            return false;
        }
        statusExist.RemainingPeople -= people;
        statusExist.TotalEvacuated += 1;
        statusExist.LastVechicleUsed = vehicle;
        if (statusExist.RemainingPeople == 0)
        {
            statusExist.IsCompleted = true;
            statusExist.Operations = "Completed";
            Log.Information($"Update Evacution Status ZoneId: {zoneId} | Operations : Completed | IsCompleted: true");
        }
        var result = await evacutionStatusRepository.UpdateEvacutionStatusAsync(statusExist.ZoneID, statusExist);
        Log.Information($"Update Evacution Status ZoneId: {zoneId} | Remove-RemainingPeople : {people} | Result-RemainingPeople: {result.RemainingPeople} ");
        var cacheHit = await cachingEvacutionStatusService.GetEvacutionStatusByZoneIdCaching(result.ZoneID);
        if (cacheHit != null)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatuseByZoneIdCaching(result.ZoneID);
            var data = new ResponseEvacutionStatusDTO(result.ZoneID, result.RemainingPeople, result.Operations, result.IsCompleted, result.LastVechicleUsed, result.TotalEvacuated);
            await cachingEvacutionStatusService.SetEvacutionStatusByZoneId(data);
        }
        return true;
    }

    public async Task<bool> UpdateRemainingReplacePeopleAsync(string zoneId, int? people, string vehicle)
    {
        var statusExist = await evacutionStatusRepository.SelectEvacutionStatusByIdAsync(zoneId);
        if (statusExist == null)
        {
            return false;
        }
        statusExist.RemainingPeople = people;
        var result = await evacutionStatusRepository.UpdateEvacutionStatusAsync(statusExist.ZoneID, statusExist);
        Log.Information($"Update Evacution Status ZoneId: {zoneId} | Add-RemainingPeople-Old : {people} ");
        var cacheHit = await cachingEvacutionStatusService.GetEvacutionStatusByZoneIdCaching(result.ZoneID);
        if (cacheHit != null)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatuseByZoneIdCaching(result.ZoneID);
            var data = new ResponseEvacutionStatusDTO(result.ZoneID, result.RemainingPeople, result.Operations, result.IsCompleted, result.LastVechicleUsed, result.TotalEvacuated);
            await cachingEvacutionStatusService.SetEvacutionStatusByZoneId(data);
        }
        return true;
    }
}
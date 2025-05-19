using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;
using EvacutionPlanningAndMonitoring.App.API.Repositories;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public class EvavacutionStatusService(IEvacutionStatusRepository evacutionStatusRepository, ICachingEvacutionStatusService cachingEvacutionStatusService) : IEvavacutionStatusService
{
    public async Task<ResponseDTO<List<EvacutionStatusDTO>>> GetEvacutionDefaultStatusesAsync()
    {
        List<EvacutionStatusDTO>? statusCaching = await cachingEvacutionStatusService.GetEvacutionStatusesCaching();
        if (statusCaching.Count != 0)
        {
            return new ResponseDTO<List<EvacutionStatusDTO>>(false, 200, statusCaching, null);
        }
        var results = await evacutionStatusRepository.SelectAllEvacutionDefualtStatusAsync();
        List<EvacutionStatusDTO>? evacutionStatusDTOs = [];
        foreach (var status in results.ToList())
        {
            var mapObject = new EvacutionStatusDTO
                (status.ZoneID,
                status.TotalEvacuated,
                status.RemainingPeople,
                status.LastVechicleUsed
                );
            evacutionStatusDTOs.Add(mapObject);
        }
        await cachingEvacutionStatusService.SetEvacutionStatusesCaching(evacutionStatusDTOs);
        return new ResponseDTO<List<EvacutionStatusDTO>>(false, 200, evacutionStatusDTOs, null);
    }

    public async Task<ResponseDTO<List<EvacutionStatusDTO>>> GetEvacutionStatusesAsync(int? page, int? limit, string? keyword)
    {

        List<EvacutionStatusDTO>? statusCaching = await cachingEvacutionStatusService.GetEvacutionStatusesCaching();
        if (statusCaching.Count != 0)
        {
            return new ResponseDTO<List<EvacutionStatusDTO>>(false, 200, statusCaching, null);
        }
        int pageDefulat = page ?? 1;
        int limitDefault = limit ?? 10;
        int skip = (pageDefulat - 1) * limitDefault;
        var results = await evacutionStatusRepository.SelectAllEvacutionStatusAsync(skip, limitDefault, keyword);
        List<EvacutionStatusDTO>? evacutionStatusDTOs = [];
        foreach (var status in results.ToList())
        {
            var mapObject = new EvacutionStatusDTO
                (status.ZoneID,
                status.TotalEvacuated,
                status.RemainingPeople,
                status.LastVechicleUsed
                );
            evacutionStatusDTOs.Add(mapObject);
        }
        await cachingEvacutionStatusService.SetEvacutionStatusesCaching(evacutionStatusDTOs);
        return new ResponseDTO<List<EvacutionStatusDTO>>(false, 200, evacutionStatusDTOs, null);
    }

    public async Task<ResponseDTO<EvacutionStatusDTO>> UpdateEvacutionStatusAsync(EvacutionStatusDTO evacutionStatusDTO)
    {
        var statusCaching = await cachingEvacutionStatusService.GetEvacutionStatusesCaching();
        var mapObject = new EvacutionStatus
        {
            ZoneID = evacutionStatusDTO.ZoneID,
            LastVechicleUsed = evacutionStatusDTO.LastVehicleUsed,
            RemainingPeople = evacutionStatusDTO.RemaininPeople,
            TotalEvacuated = evacutionStatusDTO.TotalEvacuated,
        };

        var result = await evacutionStatusRepository.UpdateEvacutionStatusAsync(evacutionStatusDTO.ZoneID, mapObject);
        if (result == null)
        {
            return new ResponseDTO<EvacutionStatusDTO>(true, 404, null, "EvacutionStatus Not Found.");

        }
        var mapResult = new EvacutionStatusDTO(result.ZoneID, result.TotalEvacuated, result.RemainingPeople, result.LastVechicleUsed);
        var results = await evacutionStatusRepository.SelectAllEvacutionDefualtStatusAsync();
        List<EvacutionStatusDTO>? evacutionStatusDTOs = [];
        foreach (var status in results.ToList())
        {
            var mapObjects = new EvacutionStatusDTO
                (status.ZoneID,
                status.TotalEvacuated,
                status.RemainingPeople,
                status.LastVechicleUsed
                );
            evacutionStatusDTOs.Add(mapObjects);
        }
        if (statusCaching.Count != 0)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatusesCaching();
            await cachingEvacutionStatusService.SetEvacutionStatusesCaching(evacutionStatusDTOs);
        }
        return new ResponseDTO<EvacutionStatusDTO>(false, 200, mapResult, null);
    }

    public async Task<bool> UpdateRemainingPeopleAsync(string zoneId, int? people, string vehicle)
    {
        var statusCaching = await cachingEvacutionStatusService.GetEvacutionStatusesCaching();
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
        await evacutionStatusRepository.UpdateEvacutionStatusAsync(statusExist.ZoneID, statusExist);
        var results = await evacutionStatusRepository.SelectAllEvacutionDefualtStatusAsync();
        List<EvacutionStatusDTO>? evacutionStatusDTOs = [];
        foreach (var status in results.ToList())
        {
            var mapObject = new EvacutionStatusDTO
                (status.ZoneID,
                status.TotalEvacuated,
                status.RemainingPeople,
                status.LastVechicleUsed
                );
            evacutionStatusDTOs.Add(mapObject);
        }
        if (statusCaching.Count != 0)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatusesCaching();
            await cachingEvacutionStatusService.SetEvacutionStatusesCaching(evacutionStatusDTOs);
        }
        return true;
    }

    public async Task<bool> UpdateRemainingReplacePeopleAsync(string zoneId, int? people, string vehicle)
    {
        var statusCaching = await cachingEvacutionStatusService.GetEvacutionStatusesCaching();
        var statusExist = await evacutionStatusRepository.SelectEvacutionStatusByIdAsync(zoneId);
        if (statusExist == null)
        {
            return false;
        }
        statusExist.RemainingPeople = people;
        await evacutionStatusRepository.UpdateEvacutionStatusAsync(statusExist.ZoneID, statusExist);
        var results = await evacutionStatusRepository.SelectAllEvacutionDefualtStatusAsync();
        List<EvacutionStatusDTO>? evacutionStatusDTOs = [];
        foreach (var status in results.ToList())
        {
            var mapObject = new EvacutionStatusDTO
                (status.ZoneID,
                status.TotalEvacuated,
                status.RemainingPeople,
                status.LastVechicleUsed
                );
            evacutionStatusDTOs.Add(mapObject);
        }
        if (statusCaching.Count != 0)
        {
            await cachingEvacutionStatusService.RemoveEvacutionStatusesCaching();
            await cachingEvacutionStatusService.SetEvacutionStatusesCaching(evacutionStatusDTOs);
        }
        return true;
    }
}
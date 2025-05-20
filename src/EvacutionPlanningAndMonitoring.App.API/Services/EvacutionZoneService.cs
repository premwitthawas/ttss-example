using System.Text.Json;
using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;
using EvacutionPlanningAndMonitoring.App.API.Repositories;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public class EvacutionZoneService(IEvacutionZoneRepository evacutionZoneRepository) : IEvacutionZoneService
{
    public async Task<ResponseDTO<EvacutionZoneDTO>> CreateEvacutionZoneAsync(EvacutionZoneDTO evacutionZoneDTO)
    {
        var mapObject = new EvacutionZone
        {
            ZoneID = evacutionZoneDTO.ZoneID,
            NumberOfPeople = evacutionZoneDTO.NumberOfPeople,
            UrgencyLevel = evacutionZoneDTO.UrgencyLevel,
            Latitude = evacutionZoneDTO.LocationCoordinates.Latitude,
            Longitude = evacutionZoneDTO.LocationCoordinates.Longitude,
            EvacutionStatus = new EvacutionStatus
            {
                RemainingPeople = evacutionZoneDTO.NumberOfPeople,
                TotalEvacuated = 0,
                IsCompleted = false,
                LastVechicleUsed = string.Empty,
                Operations = "Waiting"
            }
        };
        await evacutionZoneRepository.InsertEvacutionZoneAsync(mapObject);
        return new ResponseDTO<EvacutionZoneDTO>(false, 201, evacutionZoneDTO, null);
    }

    public async Task<bool> FindPriorityUrgencyEvacutionZoneAsync(EvacutionZone evacutionZone)
    {
        var result = await evacutionZoneRepository.SelectEvacutionZonesAsync();
        var zones = result.ToList()
        .OrderBy(x => x.CreatedAt)
        .OrderByDescending(x => x.UrgencyLevel)
        .Where(x => x.EvacutionStatus!.RemainingPeople != 0 && x.EvacutionStatus!.IsCompleted == false);
        EvacutionZone zone = zones.ToArray()[0];
        bool isFirstPriority = zone.Equals(evacutionZone);
        return isFirstPriority;
    }
}
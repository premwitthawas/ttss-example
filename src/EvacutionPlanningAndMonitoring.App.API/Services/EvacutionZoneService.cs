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
            }
        };
        await evacutionZoneRepository.InsertEvacutionZoneAsync(mapObject);
        return new ResponseDTO<EvacutionZoneDTO>(false, 201, evacutionZoneDTO, null);
    }

    public async Task<bool> FindPriorityUrgencyEvacutionZoneAsync(EvacutionZone evacutionZone)
    {
        var result = await evacutionZoneRepository.SelectEvacutionZonesAsync();
        System.Console.WriteLine(JsonSerializer.Serialize(result.ToList()));
        var zones = result.Where(x =>
        x.UrgencyLevel > evacutionZone.UrgencyLevel
        && x.EvacutionStatus!.RemainingPeople > 0)
        .ToList();
        System.Console.WriteLine(JsonSerializer.Serialize(zones));
        if (zones.Count > 0)
        {
            return false;
        }
        return true;
    }
}
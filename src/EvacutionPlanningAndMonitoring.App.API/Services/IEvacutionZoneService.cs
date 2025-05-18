using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public interface IEvacutionZoneService
{
    Task<ResponseDTO<EvacutionZoneDTO>> CreateEvacutionZoneAsync(EvacutionZoneDTO evacutionZoneDTO);
    Task<bool> FindPriorityUrgencyEvacutionZoneAsync(EvacutionZone evacutionZone);
};
using EvacutionPlanningAndMonitoring.App.API.DTOs;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public interface ICachingEvacutionStatusService
{
    Task SetEvacutionStatusesCaching(List<EvacutionStatusDTO> evacutionStatusDTOs);
    Task<List<EvacutionStatusDTO>> GetEvacutionStatusesCaching();
    Task SetEvacutionStatusByZoneId(ResponseEvacutionStatusDTO responseEvacutionStatusDTO);
    Task<ResponseEvacutionStatusDTO?> GetEvacutionStatusByZoneIdCaching(string zoneID);
    Task RemoveEvacutionStatuseByZoneIdCaching(string zoneID);
    Task RemoveEvacutionStatusesCaching();
}
using EvacutionPlanningAndMonitoring.App.API.DTOs;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public interface ICachingEvacutionStatusService
{
    Task SetEvacutionStatusesCaching(List<EvacutionStatusDTO> evacutionStatusDTOs);
    Task<List<EvacutionStatusDTO>> GetEvacutionStatusesCaching();
    Task RemoveEvacutionStatusesCaching();
}
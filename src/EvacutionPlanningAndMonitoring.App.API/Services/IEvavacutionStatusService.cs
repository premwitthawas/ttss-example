using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Services;


public interface IEvavacutionStatusService
{
    Task<ResponseDTO<List<EvacutionStatusDTO>>> GetEvacutionStatusesAsync(int? page, int? limit, string? keyword);
    Task<ResponseDTO<EvacutionStatusDTO>> UpdateEvacutionStatusAsync(EvacutionStatusDTO evacutionStatusDTO);
    Task<bool> UpdateRemainingPeopleAsync(string zoneId, int? people,string vehicle);
}
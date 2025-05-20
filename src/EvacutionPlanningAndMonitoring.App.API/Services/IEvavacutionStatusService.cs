using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Services;


public interface IEvavacutionStatusService
{
    Task<ResponseDTO<List<ResponseEvacutionStatusDTO>>> GetEvacutionDefaultStatusesAsync();
    Task<ResponseDTO<EvacutionStatusDTO>> UpdateEvacutionStatusAsync(EvacutionStatusDTO evacutionStatusDTO);
    Task<bool> UpdateRemainingPeopleAsync(string zoneId, int? people, string vehicle);
    Task<bool> UpdateRemainingReplacePeopleAsync(string zoneId, int? people,string vehicle);
    Task<bool> UpdateIsCompleteAsync(string zoneId,bool status);
    Task<bool> UpdateIsOperationsWattingsAsync(string zoneId);
    Task<bool> UpdateIsOperationsCompleteAsync(string zoneId);
}
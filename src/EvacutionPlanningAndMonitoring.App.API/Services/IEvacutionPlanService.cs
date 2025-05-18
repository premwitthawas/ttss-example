using EvacutionPlanningAndMonitoring.App.API.DTOs;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public interface IEvacutionPlanService
{
    Task<ResponseDTO<EvacutionPlanDTO>> CreateEvacutionPlanAsync(EvacutionPlanDTO evacutionPlanDTO);
    Task ClearEvacutionPlansAsync();
}
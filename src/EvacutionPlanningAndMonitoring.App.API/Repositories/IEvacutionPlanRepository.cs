using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Repositories;

public interface IEvacutionPlanRepository
{
    Task<EvacutionPlan> InsertEvacutionPlanAsync(EvacutionPlan evacutionPlan);
    Task<EvacutionPlan> UpdateEvacutionPlanAsync(EvacutionPlan evacutionPlan);
    Task<IEnumerable<EvacutionPlan>> SelectAllEvacutionPlanAsync();

}
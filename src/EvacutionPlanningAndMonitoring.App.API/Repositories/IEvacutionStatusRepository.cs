using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Repositories;

public interface IEvacutionStatusRepository
{
    Task<EvacutionStatus> UpdateEvacutionStatusAsync(string zoneId, EvacutionStatus evacutionStatus);
    Task<IEnumerable<EvacutionStatus>> SelectAllEvacutionStatusAsync(int skip, int take, string? keyword);
    Task<IEnumerable<EvacutionStatus>> SelectAllEvacutionDefualtStatusAsync();
    Task<IEnumerable<string>> SelectAllIdsEvacutionDStatusAsync();
    Task<EvacutionStatus?> SelectEvacutionStatusByIdAsync(string id);
};
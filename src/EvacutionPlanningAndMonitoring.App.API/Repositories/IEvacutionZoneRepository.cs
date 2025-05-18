using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Repositories;

public interface IEvacutionZoneRepository
{
    Task<EvacutionZone> InsertEvacutionZoneAsync(EvacutionZone evacutionZone);
    Task<EvacutionZone?> SelectEvacutionZoneByIdAsync(string id);
    Task<IEnumerable<EvacutionZone>> SelectEvacutionZonesAsync();
}
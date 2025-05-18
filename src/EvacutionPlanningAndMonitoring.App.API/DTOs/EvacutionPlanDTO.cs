namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record EvacutionPlanDTO(string ZoneID, string VehicleID, string? ETA, int? NumberOfPeople);

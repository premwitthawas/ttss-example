namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record EvacutionStatusDTO(
    string ZoneID,
    int? TotalEvacuated,
    int? RemaininPeople,
    string LastVehicleUsed
);
namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record ResponseEvacutionStatusDTO(
    string ZoneID,
    int? RemaininPeople,
    string? Operations,
    bool IsCompleted,
    string? LastVechicleUsed,
    int? TotalEvacuated
);
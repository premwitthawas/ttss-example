namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record EvacutionStatusDTO(
    string ZoneID,
    int? RemaininPeople,
    string? Operations,
    bool IsCompleted,
    string? LastVechicleUsed,
    int? TotalEvacuated
);
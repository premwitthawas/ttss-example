namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record ResponseEvacutionStatusDTO(
    string ZoneID,
    int? RemainingPeopleEvacutaed,
    string? Operations,
    bool IsCompleted,
    string? LastVechicleUsed,
    string TotalEvacuted
);
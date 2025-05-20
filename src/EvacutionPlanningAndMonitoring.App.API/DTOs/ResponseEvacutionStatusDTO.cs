using System.ComponentModel.DataAnnotations;

namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record ResponseEvacutionStatusDTO(
    [param: Required]
    string ZoneID,
    int? RemaininPeople,
    string? Operations,
    bool IsCompleted,
    string? LastVechicleUsed,
    int? TotalEvacuated
);
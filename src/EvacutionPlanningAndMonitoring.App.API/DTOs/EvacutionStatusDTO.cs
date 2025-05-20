using System.ComponentModel.DataAnnotations;

namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record EvacutionStatusDTO(
    [param: Required]
    string ZoneID,
    int? RemaininPeople,
    string? Operations,
    bool IsCompleted,
    string? LastVechicleUsed,
    int? TotalEvacuated
);
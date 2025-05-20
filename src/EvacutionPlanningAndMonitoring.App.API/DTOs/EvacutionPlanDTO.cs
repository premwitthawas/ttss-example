using System.ComponentModel.DataAnnotations;

namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record EvacutionPlanDTO(
    [param: Required]
    string ZoneID,
    [param: Required]
    string VehicleID,
    string? ETA,
    int? NumberOfPeople);

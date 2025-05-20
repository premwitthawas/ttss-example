using System.ComponentModel.DataAnnotations;

namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record EvacutionPlantUpdateDTO(
    [property: Required]
    string ZoneID,
    [property: Required]
    string VehicleID,
    int? NumberOfPeople
    );
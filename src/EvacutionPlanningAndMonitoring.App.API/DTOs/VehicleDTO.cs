using System.ComponentModel.DataAnnotations;

namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record VehicleDTO(
    [param: Required]
    string VehicleID,
    [param: Required]
    int Capacity,
    [param: Required]
    string Type,
    [param: Required]
    LocationCoordinatesDTO LocationCoordinates,
    int Speed);
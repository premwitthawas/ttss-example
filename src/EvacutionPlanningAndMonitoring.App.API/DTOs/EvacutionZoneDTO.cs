using System.ComponentModel.DataAnnotations;
using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record EvacutionZoneDTO(
    [param: Required]
    string ZoneID,
    [param: Required]
    LocationCoordinatesDTO LocationCoordinates,
    [param: Required]
    int NumberOfPeople,
    [param: Required]
    [param: Range(1,5,ErrorMessage = "UrgencyLevel is shoulde Between 1-5 level") ]
    UrgencyLevel UrgencyLevel);
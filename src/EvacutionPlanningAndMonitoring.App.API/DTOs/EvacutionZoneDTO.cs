using System.ComponentModel.DataAnnotations;
using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record EvacutionZoneDTO(
    string ZoneID,
    LocationCoordinatesDTO LocationCoordinates,
    int NumberOfPeople,
    UrgencyLevel UrgencyLevel);
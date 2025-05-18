namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record VehicleDTO(string VehicleID, int Capacity, string Type, LocationCoordinatesDTO LocationCoordinates, int Speed);
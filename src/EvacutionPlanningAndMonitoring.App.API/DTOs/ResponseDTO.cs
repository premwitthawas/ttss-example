namespace EvacutionPlanningAndMonitoring.App.API.DTOs;

public record ResponseDTO<T>(bool IsError, int StatusCode, T? Data, string? Message);
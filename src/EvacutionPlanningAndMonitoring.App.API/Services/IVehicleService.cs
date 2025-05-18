using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public interface IVehicleService
{
    Task<ResponseDTO<VehicleDTO>> CreateVehicleAsync(VehicleDTO vehicleDTO);
    Task<Vehicle?> OptimizeCapacityVehicleToZone(EvacutionZone evacutionZone);
}
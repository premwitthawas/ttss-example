using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvacutionPlanningAndMonitoring.App.API.Controllers;

[ApiController]
[Route("/api/vehicles")]
public class VehicleController(IVehicleService vehicleService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVehicle(VehicleDTO vehicleDTO)
    {
        var result = await vehicleService.CreateVehicleAsync(vehicleDTO);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
}
using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EvacutionPlanningAndMonitoring.App.API.Controllers;

[ApiController]
[Route("/api/vehicles")]
public class VehicleController(IVehicleService vehicleService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVehicle(VehicleDTO vehicleDTO)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
            var objectError = new ResponseDTO<List<string>>(false, 400, errors, null);
            return StatusCode(400, objectError);
        }
        var result = await vehicleService.CreateVehicleAsync(vehicleDTO);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
}
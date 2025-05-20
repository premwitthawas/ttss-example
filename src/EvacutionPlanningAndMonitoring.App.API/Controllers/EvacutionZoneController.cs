using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvacutionPlanningAndMonitoring.App.API.Controllers;


[ApiController]
[Route("/api/evacution-zones")]
public class EvacutionZoneController(IEvacutionZoneService evacutionZoneService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVehicle(EvacutionZoneDTO evacutionZoneDTO)
    {
         if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
            var objectError = new ResponseDTO<List<string>>(false, 400, errors, null);
            return new ObjectResult(objectError) { StatusCode = objectError.StatusCode };
        }
        var result = await evacutionZoneService.CreateEvacutionZoneAsync(evacutionZoneDTO);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
}
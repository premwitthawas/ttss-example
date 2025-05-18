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
        var result = await evacutionZoneService.CreateEvacutionZoneAsync(evacutionZoneDTO);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
}
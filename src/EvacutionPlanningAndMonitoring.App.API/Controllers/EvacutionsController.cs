using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvacutionPlanningAndMonitoring.App.API.Controllers;


[ApiController]
[Route("/api/evacutions")]
public class EvacutionsController(
    IEvacutionPlanService evacutionPlanService,
    IEvavacutionStatusService evavacutionStatusService
    ) : ControllerBase
{
    [HttpPost("plan")]
    public async Task<IActionResult> CreateVehicle(EvacutionPlanDTO evacutionPlanDTOtionZoneDTO)
    {
        var result = await evacutionPlanService.CreateEvacutionPlanAsync(evacutionPlanDTOtionZoneDTO);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus(int? page, int? limit, string? keyword)
    {
        var result = await evavacutionStatusService.GetEvacutionStatusesAsync(page, limit, keyword);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateStatus(EvacutionStatusDTO evacutionStatusDTO)
    {
        var result = await evavacutionStatusService.UpdateEvacutionStatusAsync(evacutionStatusDTO);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearPlan()
    {
        await evacutionPlanService.ClearEvacutionPlansAsync();
        return new ObjectResult(new { Message = "clear all" }) { StatusCode = 200 };
    }
}
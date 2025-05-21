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
    public async Task<IActionResult> CreatePlane(EvacutionPlanDTO evacutionPlanDTOtionZoneDTO)
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
        var result = await evacutionPlanService.CreateEvacutionPlanAsync(evacutionPlanDTOtionZoneDTO);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
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
        var result = await evavacutionStatusService.GetEvacutionDefaultStatusesAsync();
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateStatus(EvacutionPlantUpdateDTO evacutionPlantUpdateDTO)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
            var objectError = new ResponseDTO<List<string>>(false, 400, errors, null);
            return StatusCode(400,objectError);
        }
        var result = await evacutionPlanService.UpdatePlaneVehicleAndNumberOfPeopleEvacutedAsync(evacutionPlantUpdateDTO);
        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearPlan()
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
        await evacutionPlanService.ClearEvacutionPlansAsync();
        return new ObjectResult(new { Message = "clear all" }) { StatusCode = 200 };
    }
}
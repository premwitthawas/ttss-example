using System.Text.Json;
using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Helpers;
using EvacutionPlanningAndMonitoring.App.API.Models;
using EvacutionPlanningAndMonitoring.App.API.Repositories;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public class EvacutionPlanService(
    IEvacutionPlanRepository evacutionPlanRepository,
    ICalculateDistanceHelper calculateDistanceHelper,
    IEvacutionZoneRepository evacutionZoneRepository,
    IEvacutionZoneService evacutionZoneService,
    IVehicleRepository vehicleRepository,
    // IVehicleService vehicleService,
    IEvavacutionStatusService evavacutionStatusService
    ) : IEvacutionPlanService
{
    public async Task ClearEvacutionPlansAsync()
    {
        var results = await evacutionPlanRepository.SelectAllEvacutionPlanAsync();
        foreach (var plan in results)
        {
            plan.ResetPlanAndStatus();
            await evacutionPlanRepository.UpdateEvacutionPlanAsync(plan);
        }
    }

    public async Task<ResponseDTO<EvacutionPlanDTO>> CreateEvacutionPlanAsync(EvacutionPlanDTO evacutionPlanDTO)
    {
        Vehicle? vehicle = await vehicleRepository.SelectVehicleByIdAsync(evacutionPlanDTO.VehicleID);
        if (vehicle == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(false, 404, null, "Vehicle Not found.");
        }
        EvacutionZone? evacutionZone = await evacutionZoneRepository.SelectEvacutionZoneByIdAsync(evacutionPlanDTO.ZoneID);
        if (evacutionZone == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(false, 404, null, "EvacutionZone Not found.");
        }
        bool isPriorityZoneCanUse = await evacutionZoneService.FindPriorityUrgencyEvacutionZoneAsync(evacutionZone);
        if (!isPriorityZoneCanUse)
        {
            return new ResponseDTO<EvacutionPlanDTO>(false, 400, null, "Please assign vehicle to zone with higher urgency level first.");
        }
        if (evacutionPlanDTO.NumberOfPeople == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(false, 400, null, "NumberOfPeople is Null");
        }
        bool isUpdatePeopleRemaining = await evavacutionStatusService.UpdateRemainingPeopleAsync(evacutionZone.ZoneID, evacutionPlanDTO.NumberOfPeople, vehicle.VehicleID);
        if (!isUpdatePeopleRemaining)
        {
            return new ResponseDTO<EvacutionPlanDTO>(false, 400, null, "Can't Update Evacution Status.");
        }
        // var vehicleOptimize = await vehicleService.OptimizeCapacityVehicleToZone(evacutionZone);
        // if (vehicleOptimize != null)
        // {
        //     System.Console.WriteLine(JsonSerializer.Serialize(vehicleOptimize));
        // }
        var mapObject = new EvacutionPlan
        {
            ZoneID = evacutionPlanDTO.ZoneID,
            VehicleID = evacutionPlanDTO.VehicleID,
            NumberOfPeople = evacutionPlanDTO.NumberOfPeople,
            ETA = calculateDistanceHelper.CalculateETAOfEvacutionPlan(vehicle, evacutionZone),
        };
        var result = await evacutionPlanRepository.InsertEvacutionPlanAsync(mapObject);
        var mapResult = new EvacutionPlanDTO(result.ZoneID, result.VehicleID, result.ETA, result.NumberOfPeople);
        return new ResponseDTO<EvacutionPlanDTO>(false, 201, mapResult, null);
    }
}

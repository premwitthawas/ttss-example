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
    IVehicleService vehicleService,
    IEvavacutionStatusService evavacutionStatusService
    ) : IEvacutionPlanService
{
    public async Task ClearEvacutionPlansAsync()
    {
        var results = await evacutionPlanRepository.SelectAllEvacutionPlanAsync();
        var list = results.ToList();
        foreach (var plan in list)
        {
            if (plan.EvacutionZone != null)
            {
                await evavacutionStatusService.UpdateRemainingReplacePeopleAsync(plan.ZoneID, plan.EvacutionZone.NumberOfPeople, plan.VehicleID);
                await vehicleService.UpdateVehicleStatusAsync(plan.VehicleID, false);
                plan.ResetPlan();
                await evacutionPlanRepository.UpdateEvacutionPlanAsync(plan);
            }
        }
    }

    public async Task<ResponseDTO<EvacutionPlanDTO>> CreateEvacutionPlanAsync(EvacutionPlanDTO evacutionPlanDTO)
    {
        Vehicle? vehicle = await vehicleRepository.SelectVehicleByIdAsync(evacutionPlanDTO.VehicleID);
        if (vehicle == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 404, null, "Vehicle Not found.");
        }
        EvacutionZone? evacutionZone = await evacutionZoneRepository.SelectEvacutionZoneByIdAsync(evacutionPlanDTO.ZoneID);
        if (evacutionZone == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 404, null, "EvacutionZone Not found.");
        }
        if (vehicle.Capacity < evacutionPlanDTO.NumberOfPeople)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "Vehicle Capacity it's less then NumberOfPeopleEvacuted");
        }
        Vehicle? vehicleOptimize = await vehicleService.OptimizeCapacityVehicleToZone(evacutionZone, evacutionPlanDTO.NumberOfPeople);
        if (vehicleOptimize == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "Vehicle is not avaliable please try again next time.");
        }
        bool isVehicleOptimize = vehicleOptimize.Equals(vehicle);
        if (!isVehicleOptimize)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "Please Assign Vehicle " + vehicleOptimize.VehicleID);
        }
        bool isPriorityZoneCanUse = await evacutionZoneService.FindPriorityUrgencyEvacutionZoneAsync(evacutionZone);
        if (!isPriorityZoneCanUse)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "Please assign vehicle to zone with higher urgency level first.");
        }
        var mapObject = new EvacutionPlan
        {
            ZoneID = evacutionPlanDTO.ZoneID,
            VehicleID = evacutionPlanDTO.VehicleID,
            NumberOfPeople = vehicle.Capacity,
            ETA = calculateDistanceHelper.CalculateETAOfEvacutionPlan(vehicle, evacutionZone),
        };
        var planExist = await evacutionPlanRepository.SelectEvacutionPlanByIdAsync(evacutionPlanDTO.ZoneID, evacutionPlanDTO.VehicleID);
        if (planExist != null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "EvacutionPlan Is Exists.");
        }
        var result = await evacutionPlanRepository.InsertEvacutionPlanAsync(mapObject);
        bool isUpdatePeopleRemaining = await evavacutionStatusService.UpdateRemainingPeopleAsync(evacutionZone.ZoneID, vehicle.Capacity, vehicle.VehicleID);
        if (!isUpdatePeopleRemaining)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "Can't Update Evacution Status.");
        }
        bool isUpdateStatusVehicle = await vehicleService.UpdateVehicleStatusAsync(vehicle.VehicleID, true);
        if (!isUpdateStatusVehicle)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "Can't Update Vehicle Status.");
        }
        var mapResult = new EvacutionPlanDTO(result.ZoneID, result.VehicleID, result.ETA, result.NumberOfPeople);
        return new ResponseDTO<EvacutionPlanDTO>(false, 201, mapResult, null);
    }

    public async Task<ResponseDTO<EvacutionPlanDTO>> UpdatePlaneVehicleAndNumberOfPeopleEvacutedAsync(EvacutionPlantUpdateDTO evacutionPlantUpdateDTO)
    {
        Vehicle? vehicle = await vehicleRepository.SelectVehicleByIdAsync(evacutionPlantUpdateDTO.VehicleID);
        if (vehicle == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 404, null, "Vehicle Not found.");
        }
        if (vehicle.IsUsed)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "Vehicle is not avaliable please try again next time.");
        }
        EvacutionZone? evacutionZone = await evacutionZoneRepository.SelectEvacutionZoneByIdAsync(evacutionPlantUpdateDTO.ZoneID);
        if (evacutionZone == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 404, null, "EvacutionZone Not found.");
        }
        var planExist = await evacutionPlanRepository.SelectEvacutionPlanByIdAsync(evacutionPlantUpdateDTO.ZoneID, evacutionPlantUpdateDTO.VehicleID);
        if (planExist == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 404, null, "EvacutionPlan Not found | Please Add EvacutionPlan Before Update.");
        }
        planExist.NumberOfPeople = evacutionPlantUpdateDTO.NumberOfPeople;
        planExist.ZoneID = evacutionPlantUpdateDTO.ZoneID ?? planExist.ZoneID;
        planExist.VehicleID = evacutionPlantUpdateDTO.VehicleID;
        if (planExist.Vehicle == null && planExist.EvacutionZone == null)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 404, null, "Vehicle Or EvacutionZone Not found in Plan.");
        }
        planExist.ETA = calculateDistanceHelper.CalculateETAOfEvacutionPlan(vehicle, evacutionZone);
        var result = await evacutionPlanRepository.UpdateEvacutionPlanAsync(planExist);
        bool isUpdatePeopleRemaining = await evavacutionStatusService.UpdateRemainingPeopleAsync(result.ZoneID, vehicle.Capacity, result.VehicleID);
        if (!isUpdatePeopleRemaining)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "People remaining less than base capacity of vehicle");
        }
        bool isUpdateStatusVehicle = await vehicleService.UpdateVehicleStatusAsync(vehicle.VehicleID, true);
        if (!isUpdateStatusVehicle)
        {
            return new ResponseDTO<EvacutionPlanDTO>(true, 400, null, "Can't Update Vehicle Status.");
        }
        var mapResult = new EvacutionPlanDTO(result.ZoneID, result.VehicleID, result.ETA, result.NumberOfPeople);
        return new ResponseDTO<EvacutionPlanDTO>(false, 200, mapResult, null);
    }
}

using System.Text.Json;
using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;
using EvacutionPlanningAndMonitoring.App.API.Repositories;

namespace EvacutionPlanningAndMonitoring.App.API.Services;

public class VehicleService(IVehicleRepository vehicleRepository) : IVehicleService
{
    public async Task<ResponseDTO<VehicleDTO>> CreateVehicleAsync(VehicleDTO vehicleDTO)
    {
        var mapObject = new Vehicle
        {
            VehicleID = vehicleDTO.VehicleID,
            Speed = vehicleDTO.Speed,
            Type = vehicleDTO.Type,
            Capacity = vehicleDTO.Capacity,
            Latitude = vehicleDTO.LocationCoordinates.Latitude,
            Longitude = vehicleDTO.LocationCoordinates.Longitude,
        };
        await vehicleRepository.InsertVehicleAsync(mapObject);
        return new ResponseDTO<VehicleDTO>(false, 201, vehicleDTO, null);
    }

    public async Task<Vehicle?> OptimizeCapacityVehicleToZone(EvacutionZone evacutionZone, int? capacity)
    {
            int? numberOfPeople = evacutionZone.EvacutionStatus!.RemainingPeople;
            if (numberOfPeople == null)
            {
                return null;
            }
            var result = await vehicleRepository.SelectVehiclesAsync();
            var vehiclesCapatity = result.Where(x => x.IsUsed == false).ToArray();
            if (vehiclesCapatity.Length == 0)
            {
                return null;
            }
            Vehicle res = vehiclesCapatity[0];
            for (int i = 1; i < vehiclesCapatity.Length; i++)
            {
            if (Math.Abs(vehiclesCapatity[i].Capacity - (int)numberOfPeople) <= Math.Abs(res.Capacity - (int)numberOfPeople))
            {
                res = vehiclesCapatity[i];
            }
            else
            {
                res = vehiclesCapatity[0];
            }
            }
            return res;
    }

    public async Task<bool> UpdateVehicleStatusAsync(string id, bool status)
    {
        return await vehicleRepository.UpdateIsUsedOfVehicleAsync(id, status);
    }
}
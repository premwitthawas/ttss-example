using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;
using EvacutionPlanningAndMonitoring.App.API.Repositories;
using EvacutionPlanningAndMonitoring.App.API.Services;
using Moq;

namespace EvacutionPlanningAndMonitoring.App.Tests.Services;


public class VehicleServiceTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepoMoc;
    private readonly VehicleService _service;
    public VehicleServiceTests()
    {
        this._vehicleRepoMoc = new Mock<IVehicleRepository>();
        this._service = new VehicleService(_vehicleRepoMoc.Object);
    }
    [Fact]
    public async Task CreateVehicleSuccess()
    {
        Vehicle vehicle = new Vehicle
        {
            VehicleID = "V1",
            Capacity = 40,
            Type = "bus",
            IsUsed = false,
            Latitude = 13.5600,
            Longitude = 100.7500,
        };
        _vehicleRepoMoc.Setup(r => r.InsertVehicleAsync(vehicle)).ReturnsAsync(vehicle);
        var mapObject = new VehicleDTO(vehicle.VehicleID, vehicle.Capacity, vehicle.Type, new LocationCoordinatesDTO(vehicle.Latitude, vehicle.Longitude), vehicle.Speed);
        var assertResult = new ResponseDTO<VehicleDTO>(false, 201, mapObject, null);
        var result = await _service.CreateVehicleAsync(mapObject);
        Assert.Equal(assertResult, result);
    }
}
using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Helpers;
using EvacutionPlanningAndMonitoring.App.API.Models;
using EvacutionPlanningAndMonitoring.App.API.Repositories;
using Moq;

namespace EvacutionPlanningAndMonitoring.App.API.Services;


public class EvacutionPlanServiceTests
{
    private readonly Mock<IEvacutionPlanRepository> _mockPlanRepo;
    private readonly Mock<IVehicleService> _mockVehicleService;
    private readonly Mock<IEvacutionZoneService> _mockEvacutionZoneService;
    private readonly Mock<IEvavacutionStatusService> _mockEvavacutionStatusService;
    private readonly Mock<ICalculateDistanceHelper> _mockCalculateDistanceHelper;
    private readonly Mock<ICachingEvacutionStatusService> _mockCachingEvacutionStatusService;
    private readonly EvacutionPlanService _servicePlane;
    public EvacutionPlanServiceTests()
    {
        this._mockPlanRepo = new Mock<IEvacutionPlanRepository>();
        this._mockVehicleService = new Mock<IVehicleService>();
        this._mockEvacutionZoneService = new Mock<IEvacutionZoneService>();
        this._mockEvavacutionStatusService = new Mock<IEvavacutionStatusService>();
        this._mockCalculateDistanceHelper = new Mock<ICalculateDistanceHelper>();
        this._mockCachingEvacutionStatusService = new Mock<ICachingEvacutionStatusService>();

        this._servicePlane = new EvacutionPlanService(
            _mockPlanRepo.Object,
            _mockCalculateDistanceHelper.Object,
            _mockEvacutionZoneService.Object,
            _mockVehicleService.Object,
            _mockEvavacutionStatusService.Object,
            _mockCachingEvacutionStatusService.Object
            );
    }

    [Fact]
    public async Task CreateEvacutionPlanSucess()
    {
        EvacutionZone evacutionZone = new()
        {
            ZoneID = "Z1",
            Latitude = 13.5500,
            Longitude = 100.7500,
            UrgencyLevel = UrgencyLevel.Level5,
            NumberOfPeople = 100,
            EvacutionStatus = new EvacutionStatus
            {
                IsCompleted = false,
                Operations = "Waiting",
                LastVechicleUsed = "",
                TotalEvacuated = 0,
                RemainingPeople = 100
            }
        };
        Vehicle vehicle = new()
        {
            VehicleID = "V1",
            Capacity = 40,
            Type = "bus",
            IsUsed = false,
            Latitude = 13.5600,
            Longitude = 100.7500,
        };
        EvacutionPlan plan = new()
        {
            ETA = "10 minutes",
            NumberOfPeople = vehicle.Capacity,
            VehicleID = vehicle.VehicleID,
            ZoneID = evacutionZone.ZoneID
        };

        _mockVehicleService.Setup(x => x.GetVehicleByIdAsync(vehicle.VehicleID)).ReturnsAsync(vehicle);
        _mockEvacutionZoneService.Setup(x => x.GetEvacutionZoneByIdAsync(evacutionZone.ZoneID)).ReturnsAsync(evacutionZone);
        _mockVehicleService.Setup(x => x.OptimizeCapacityVehicleToZone(evacutionZone, vehicle.Capacity)).ReturnsAsync(vehicle);
        _mockEvacutionZoneService.Setup(x => x.FindPriorityUrgencyEvacutionZoneAsync(evacutionZone)).ReturnsAsync(true);
        _mockCalculateDistanceHelper.Setup(x => x.CalculateETAOfEvacutionPlan(vehicle, evacutionZone)).Returns("10 minutes");
        _mockPlanRepo.Setup(x => x.SelectEvacutionPlanByIdAsync(vehicle.VehicleID, evacutionZone.ZoneID)).ReturnsAsync((EvacutionPlan?)null);
        _mockPlanRepo.Setup(x => x.InsertEvacutionPlanAsync(plan)).ReturnsAsync(plan);
        _mockEvavacutionStatusService.Setup(x => x.UpdateRemainingPeopleAsync(evacutionZone.ZoneID, vehicle.Capacity, vehicle.VehicleID)).ReturnsAsync(true);
        _mockVehicleService.Setup(x => x.UpdateVehicleStatusAsync(vehicle.VehicleID, true))
        .Callback<string, bool>((id, flag) =>
    {
        Console.WriteLine($"Mock called with ID: {id}, Flag: {flag}");
    })
        .ReturnsAsync(true);
        var mapObject = new EvacutionPlanDTO(evacutionZone.ZoneID, vehicle.VehicleID, "10 minutes", vehicle.Capacity);
        //var result = await _servicePlane.CreateEvacutionPlanAsync(mapObject);
        //var assertResult = new ResponseDTO<EvacutionPlanDTO>(false, 201, mapObject, null);
        // Assert.Equal(assertResult, result);
        // Assert.Equal(assertResult.StatusCode, result.StatusCode);

    }
}
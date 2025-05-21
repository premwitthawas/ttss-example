using EvacutionPlanningAndMonitoring.App.API.DTOs;
using EvacutionPlanningAndMonitoring.App.API.Models;
using EvacutionPlanningAndMonitoring.App.API.Repositories;
using EvacutionPlanningAndMonitoring.App.API.Services;
using Moq;

namespace EvacutionPlanningAndMonitoring.App.Tests.Services;

public class EvacutionZoneServiceTests
{
    private readonly Mock<IEvacutionZoneRepository> _mock;
    private readonly EvacutionZoneService _service;
    public EvacutionZoneServiceTests()
    {
        this._mock = new Mock<IEvacutionZoneRepository>();
        this._service = new EvacutionZoneService(_mock.Object);
    }

    [Fact]
    public async Task CreateEvacutionZoneSuccess()
    {
        EvacutionZone evacutionZone = new EvacutionZone
        {
            ZoneID = "Z1",
            Latitude = 13.5500,
            Longitude = 100.7500,
            UrgencyLevel = UrgencyLevel.Level5,
            NumberOfPeople = 20,
        };
        _mock.Setup(x => x.InsertEvacutionZoneAsync(evacutionZone)).ReturnsAsync(evacutionZone);
        var mapObject = new EvacutionZoneDTO(evacutionZone.ZoneID, new LocationCoordinatesDTO(evacutionZone.Latitude, evacutionZone.Longitude), evacutionZone.NumberOfPeople, UrgencyLevel.Level5);
        var result = await _service.CreateEvacutionZoneAsync(mapObject);
        var assertResult = new ResponseDTO<EvacutionZoneDTO>(false, 201, mapObject, null);
        Assert.Equal(assertResult,result);
    }
}
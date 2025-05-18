namespace EvacutionPlanningAndMonitoring.App.API.Models;

public class EvacutionPlan : BaseModelDate
{
    public string ZoneID { get; set; } = string.Empty;
    public EvacutionZone? EvacutionZone { get; set; }
    public string VehicleID { get; set; } = string.Empty;
    public Vehicle? Vehicle { get; set; }
    public string ETA { get; set; } = string.Empty;
    public int? NumberOfPeople { get; set; }

    public void ResetPlanAndStatus()
    {
        this.ETA = string.Empty;
        this.NumberOfPeople = 0;
    }
}
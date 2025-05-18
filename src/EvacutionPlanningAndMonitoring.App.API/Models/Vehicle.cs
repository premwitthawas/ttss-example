using System.ComponentModel.DataAnnotations;

namespace EvacutionPlanningAndMonitoring.App.API.Models;

public class Vehicle : BaseModelDate
{
    [Key]
    public string VehicleID { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Type { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Speed { get; set; }
    public bool IsUsed { get; set; } = false;
    public List<EvacutionPlan>? EvacutionPlans { get; set; }
}
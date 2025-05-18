using System.ComponentModel.DataAnnotations;

namespace EvacutionPlanningAndMonitoring.App.API.Models;

public class EvacutionZone : BaseModelDate
{
    [Key]
    public string ZoneID { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int NumberOfPeople { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
    public EvacutionStatus? EvacutionStatus { get; set; }
    public List<EvacutionPlan>? EvacutionPlans { get; set; }
}
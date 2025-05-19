using System.ComponentModel.DataAnnotations;

namespace EvacutionPlanningAndMonitoring.App.API.Models
{
    public class EvacutionStatus
    {
        [Key]
        public string ZoneID { get; set; } = string.Empty;
        public int? TotalEvacuated { get; set; }
        public int? RemainingPeople { get; set; }
        public string LastVechicleUsed { get; set; } = string.Empty;
        public string Operations { get; set; } = "Waiting";
        public bool IsCompleted { get; set; } = false;
        public EvacutionZone? EvacutionZone { get; set; }

    }
}
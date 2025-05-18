using System.ComponentModel.DataAnnotations.Schema;

namespace EvacutionPlanningAndMonitoring.App.API.Models;


public class BaseModelDate
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}


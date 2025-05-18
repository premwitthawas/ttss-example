using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Helpers;

public interface ICalculateDistanceHelper
{
    double CalculateDistanceByHaversineFormula(double latitude1, double longitude1, double latitude2, double longitude2);
    string CalculateETAOfEvacutionPlan(Vehicle vehicle, EvacutionZone evacutionZone);
}
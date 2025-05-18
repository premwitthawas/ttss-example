using EvacutionPlanningAndMonitoring.App.API.Models;

namespace EvacutionPlanningAndMonitoring.App.API.Helpers;

public class CalculateDistanceHelper : ICalculateDistanceHelper
{
    public double CalculateDistanceByHaversineFormula(double latitude1, double longitude1, double latitude2, double longitude2)
    {
        double dLat = (Math.PI / 180) * (latitude2 - latitude1);
        double dLon = (Math.PI / 180) * (longitude2 - longitude1);

        latitude1 = (Math.PI / 180) * (latitude1);
        latitude2 = (Math.PI / 180) * (latitude2);

        double a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Pow(Math.Sin(dLon / 2), 2) * Math.Cos(latitude1) * Math.Cos(latitude2);
        double rad = 6371;
        double c = 2 * Math.Asin(Math.Sqrt(a));
        return rad * c;
    }

    public string CalculateETAOfEvacutionPlan(Vehicle vehicle, EvacutionZone evacutionZone)
    {
        double distance = this.CalculateDistanceByHaversineFormula(
            vehicle.Latitude,
            vehicle.Longitude,
            evacutionZone.Latitude,
            evacutionZone.Longitude
            );

        double minutes = (distance / vehicle.Speed) * 60;
        double roundedMinutes = Math.Round(minutes, 1);
        return roundedMinutes.ToString() + " " + "minutes";
    }
}

using EvacutionPlanningAndMonitoring.App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EvacutionPlanningAndMonitoring.App.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<EvacutionZone> EvacutionZones { get; set; }
    public DbSet<EvacutionPlan> EvacutionPlans { get; set; }
    public DbSet<Vehicle> Vehicle { get; set; }
    public DbSet<EvacutionStatus> EvacutionStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EvacutionZone>().HasOne(x => x.EvacutionStatus).WithOne(x => x.EvacutionZone).HasForeignKey<EvacutionStatus>(x => x.ZoneID);
        modelBuilder.Entity<EvacutionPlan>().HasKey(x => new { x.ZoneID, x.VehicleID });
        modelBuilder.Entity<EvacutionPlan>().HasOne(x => x.EvacutionZone).WithMany(x => x.EvacutionPlans).HasForeignKey(x => x.ZoneID);
        modelBuilder.Entity<EvacutionPlan>().HasOne(x => x.Vehicle).WithMany(x => x.EvacutionPlans).HasForeignKey(x => x.VehicleID);
    }
}

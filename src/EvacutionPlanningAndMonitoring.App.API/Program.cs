using dotenv.net;
using EvacutionPlanningAndMonitoring.App.API.Data;
using EvacutionPlanningAndMonitoring.App.API.Extensions;
using EvacutionPlanningAndMonitoring.App.API.Middlewares;
using Microsoft.EntityFrameworkCore;
DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(cfg =>
{
    string? connectionStrings = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrEmpty(connectionStrings)) throw new Exception("DATABASE_URL ENV not set.");
    cfg.UseNpgsql(connectionStrings);
});
builder.Services.AddRepositoriesDIExtension();
builder.Services.AddServiceDIExtension();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

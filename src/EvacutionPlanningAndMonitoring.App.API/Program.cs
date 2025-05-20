using dotenv.net;
using EvacutionPlanningAndMonitoring.App.API.Data;
using EvacutionPlanningAndMonitoring.App.API.Extensions;
using EvacutionPlanningAndMonitoring.App.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;
DotEnv.Load();
var log = new LoggerConfiguration()
    .WriteTo.File(new CompactJsonFormatter(), "./Logs/log.json", rollingInterval: RollingInterval.Day)
    .CreateLogger();
Log.Logger = log;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(cfg =>
{
    string? connectionStrings = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrEmpty(connectionStrings)) throw new Exception("DATABASE_URL ENV not set.");
    cfg.UseNpgsql(connectionStrings);
});
builder.Services.AddStackExchangeRedisCache(cfg =>
{
    string? redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION");
    if (string.IsNullOrEmpty(redisConnection)) throw new Exception("REDIS_CONNECTION ENV not set.");
    cfg.Configuration = redisConnection;
});
builder.Services.AddRepositoriesDIExtension();
builder.Services.AddServiceDIExtension();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseMiddleware<GlobalExceptionMiddleware>();
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

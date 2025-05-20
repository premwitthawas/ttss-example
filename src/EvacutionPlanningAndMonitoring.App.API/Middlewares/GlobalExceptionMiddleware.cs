using EvacutionPlanningAndMonitoring.App.API.DTOs;

namespace EvacutionPlanningAndMonitoring.App.API.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (KeyNotFoundException notfoundEx)
        {
            var body = new ResponseDTO<string?>(true, 404, null, notfoundEx.Message);
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(body);
        }
        catch (Exception ex)
        {
            ResponseDTO<string?> body;
            if (ex.InnerException != null)
            {
                body = new ResponseDTO<string?>(true, 500, null, ex.InnerException.Message);
            }
            else
            {
                body = new ResponseDTO<string?>(true, 500, null, ex.Message);
            }
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(body);
        }
    }
}
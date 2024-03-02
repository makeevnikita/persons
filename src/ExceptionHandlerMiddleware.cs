using Skills.Exceptions;
using System.Text.Json;



namespace Skills;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation(context.Request.Path);

            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogDebug(ex.Message);

            context.Response.StatusCode = 404;

            await context.Response.WriteAsJsonAsync(new { result = ex.Message });
        }
        catch (AlreadyExistsException ex)
        {
            _logger.LogDebug(ex.Message);

            context.Response.StatusCode = 409;

            await context.Response.WriteAsJsonAsync(new { result = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");

            context.Response.StatusCode = 500;

            await context.Response.WriteAsJsonAsync(new { result = "Internal Error" });
        }
    }
}
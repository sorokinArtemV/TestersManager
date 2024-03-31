using Serilog;

namespace TestersManager.UI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IDiagnosticContext diagnosticContext)
    {
        _next = next;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            if (ex.InnerException is not null)
                _logger.LogError("{ExceptionType}.{ExceptionMessage}",
                    ex.InnerException.GetType().Name,
                    ex.InnerException.Message);
            else
                _logger.LogError("{ExceptionType}.{ExceptionMessage}",
                    ex.GetType().Name,
                    ex.Message);

            // httpContext.Response.StatusCode = 500;
            // await httpContext.Response.WriteAsync("Error occured");

            throw;
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
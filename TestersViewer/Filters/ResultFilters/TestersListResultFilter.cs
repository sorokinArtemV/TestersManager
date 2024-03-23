using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersViewer.Filters.ResultFilters;

public class TestersListResultFilter : IAsyncResultFilter
{
    private readonly ILogger<TestersListResultFilter> _logger;

    public TestersListResultFilter(ILogger<TestersListResultFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        _logger.LogInformation("{FilterName}.{MethodName} - before", 
            nameof(TestersListResultFilter),
            nameof(OnResultExecutionAsync));

        context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
        
        await next();
        
        _logger.LogInformation("{FilterName}.{MethodName} - after",
            nameof(TestersListResultFilter),
            nameof(OnResultExecutionAsync));
        
    }
}
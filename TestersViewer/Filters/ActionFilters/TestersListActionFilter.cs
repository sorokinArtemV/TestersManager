using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersViewer.Filters.ActionFilters;

public class TestersListActionFilter : IActionFilter
{
    private readonly ILogger<TestersListActionFilter> _logger;

    public TestersListActionFilter(ILogger<TestersListActionFilter> logger)
    {
        _logger = logger;
    }


    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("OnActionExecuting method of TestersListActionFilter invoked");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("OnActionExecuted method of TestersListActionFilter invoked");
    }
}
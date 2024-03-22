using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersViewer.Filters.ActionFilters;

public class ResponseHeaderActionFilter : IActionFilter
{
    private readonly ILogger<ResponseHeaderActionFilter> _logger;
    private readonly string _key;
    private readonly string _value;


    public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value)
    {
        _logger = logger;
        _key = key;
        _value = value;
    }


    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName}",
            nameof(ResponseHeaderActionFilter),
            nameof(OnActionExecuting));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        context.HttpContext.Response.Headers[_key] = _value;
    }
}
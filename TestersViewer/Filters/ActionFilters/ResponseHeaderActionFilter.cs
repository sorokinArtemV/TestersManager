using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersViewer.Filters.ActionFilters;

public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
{
    private readonly string _key;
    private readonly ILogger<ResponseHeaderActionFilter> _logger;
    private readonly string _value;

    public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order)
    {
        _logger = logger;
        _key = key;
        _value = value;
        Order = order;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method before",
            nameof(ResponseHeaderActionFilter),
            nameof(OnActionExecutionAsync));

        await next(); // calls next filter or action method

        _logger.LogInformation("{FilterName}.{MethodName} method after",
            nameof(ResponseHeaderActionFilter),
            nameof(OnActionExecutionAsync));

        context.HttpContext.Response.Headers[_key] = _value;
    }

    public int Order { get; }
}
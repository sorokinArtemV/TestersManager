using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersViewer.Filters.ActionFilters;

public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
{
    public ResponseHeaderFilterFactoryAttribute(string? key, string? value, int order)
    {
        Key = key;
        Value = value;
        Order = order;
    }

    private string? Key { get; }
    private string? Value { get; }
    private int Order { get; }
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
        filter.Key = Key;
        filter.Value = Value;
        filter.Order = Order;

        return filter;
    }
}

public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
{
    private readonly ILogger<ResponseHeaderActionFilter> _logger;

    public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
    {
        _logger = logger;
    }

    public string Key { get; set; }
    public string Value { get; set; }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("Before logic ResponseHeaderActionFilter");

        await next(); // calls next filter or action method

        _logger.LogInformation("After logic ResponseHeaderActionFilter");

        context.HttpContext.Response.Headers.Append(Key, Value);
    }

    public int Order { get; set; }
}
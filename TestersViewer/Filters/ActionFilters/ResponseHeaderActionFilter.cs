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
        var filter = new ResponseHeaderActionFilter(Key, Value, Order);

        return filter;
    }
}

public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
{
    private readonly string _key;
    private readonly string _value;

    public ResponseHeaderActionFilter(string key, string value, int order)
    {
        _key = key;
        _value = value;
        Order = order;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next(); // calls next filter or action method

        context.HttpContext.Response.Headers[_key] = _value;
    }

    public int Order { get; }
}
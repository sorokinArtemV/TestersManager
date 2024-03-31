using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersManager.UI.Filters.ResultFilters;

public class TokenResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
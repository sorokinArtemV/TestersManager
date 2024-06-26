using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersManager.UI.Filters.ResultFilters;

public class TestersAlwaysRunResultFilter : IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        if (context.Filters.OfType<SkipFilter>().Any()) return;
    }
}
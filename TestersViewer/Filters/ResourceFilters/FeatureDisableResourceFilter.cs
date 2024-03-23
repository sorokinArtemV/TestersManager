using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersViewer.Filters.ResourceFilters;

public class FeatureDisableResourceFilter : IAsyncResourceFilter
{
    private readonly bool _isDisabled;
    private readonly ILogger<FeatureDisableResourceFilter> _logger;


    public FeatureDisableResourceFilter(ILogger<FeatureDisableResourceFilter> logger, bool isDisabled = true)
    {
        _logger = logger;
        _isDisabled = isDisabled;
    }


    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        _logger.LogInformation("{FilterName}.{MethodName} - before",
            nameof(FeatureDisableResourceFilter),
            nameof(OnResourceExecutionAsync));

        if (_isDisabled)
            context.Result = new StatusCodeResult(501);
        else
            await next();

        _logger.LogInformation("{FilterName}.{MethodName} - after",
            nameof(FeatureDisableResourceFilter),
            nameof(OnResourceExecutionAsync));
    }
}
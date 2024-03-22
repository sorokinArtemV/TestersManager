using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

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

        if (!context.ActionArguments.TryGetValue("searchBy", out var argument)) return;

        var searchBy = argument as string;

        if (string.IsNullOrEmpty(searchBy)) return;
        
        List<string> searchByOptions =
        [
            nameof(TesterResponse.TesterName),
            nameof(TesterResponse.Email),
            nameof(TesterResponse.DevStream),
            nameof(TesterResponse.Position),
            nameof(TesterResponse.Skills),
            nameof(TesterResponse.Age),
            nameof(TesterResponse.Gender),
            nameof(TesterResponse.MonthsOfWorkExperience)
        ];

        if (searchByOptions.Any(x => x == searchBy)) return;
        
        // reset search param
        _logger.LogInformation("searchBy value is {searchBy}", searchBy);
        context.ActionArguments["searchBy"] = nameof(TesterResponse.TesterName);
        _logger.LogInformation("searchBy updated value is {searchBy}", searchBy);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("OnActionExecuted method of TestersListActionFilter invoked");
    }
}
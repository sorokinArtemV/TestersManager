using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using TestersViewer.Controllers;

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
        context.HttpContext.Items["arguments"] = context.ActionArguments;

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
        _logger.LogInformation("searchBy updated value is {searchBy}", context.ActionArguments["searchBy"]);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("OnActionExecuted method of TestersListActionFilter invoked");

        var testersController = (TestersController)context.Controller;

        if (context.HttpContext.Items["arguments"] is not Dictionary<string, object?> parameters ||
            !parameters.TryGetValue("searchBy", out var value)) return;

        testersController.ViewData["CurrentSearchBy"] = Convert.ToString(value);

        if (parameters.TryGetValue("searchString", out var searchParam))
            testersController.ViewData["CurrentSearchString"] = Convert.ToString(searchParam);

        if (parameters.TryGetValue("sortBy", out var sortByParam))
            testersController.ViewData["CurrentSortBy"] = Convert.ToString(sortByParam);

        if (parameters.TryGetValue("sortOrder", out var sortOrderParam))
            testersController.ViewData["CurrentSortOrder"] = Convert.ToString(sortOrderParam);
    }
}
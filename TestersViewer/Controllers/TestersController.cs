using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace TestersViewer.Controllers;

public class TestersController : Controller
{
    private readonly IDevStreamsService _devStreamsService;
    private readonly ITestersService _testersService;

    public TestersController(ITestersService testersService, IDevStreamsService devStreamsService)
    {
        _testersService = testersService;
        _devStreamsService = devStreamsService;
    }

    [Route("testers/index")]
    [Route("/")]
    public IActionResult Index(
        string searchBy,
        string? searchString,
        string sortBy = nameof(TesterResponse.TesterName),
        SortOrderOptions sortOrder = SortOrderOptions.Asc)
    {
        // Search
        ViewBag.SearchFields = new Dictionary<string, string>
        {
            [nameof(TesterResponse.TesterName)] = "Name",
            [nameof(TesterResponse.DevStream)] = "Stream",
            [nameof(TesterResponse.Position)] = "Position",
            [nameof(TesterResponse.Skills)] = "Skills",
            [nameof(TesterResponse.Age)] = "Age",
            [nameof(TesterResponse.Email)] = "Email",
            [nameof(TesterResponse.Gender)] = "Gender",
            [nameof(TesterResponse.MonthsOfWorkExperience)] = "Works for"
        };

        var testers = _testersService.GetFilteredTesters(searchBy, searchString);

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        // Sort
        var sortedTesters = _testersService.GetSortedTesters(testers, sortBy, sortOrder);

        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder.ToString();

        return View(sortedTesters);
    }


    // triggers on click create
    [Route("testers/create")]
    [HttpGet]
    public IActionResult Create()
    {
        var devStreams = _devStreamsService.GetAllDevStreams();
        ViewBag.DevStreams = devStreams;

        return View();
    }
}
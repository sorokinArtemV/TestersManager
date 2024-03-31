using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestersManager.Core.DTO;
using TestersManager.Core.Enums;
using TestersManager.Core.ServiceContracts;
using TestersManager.UI.Filters;
using TestersManager.UI.Filters.ActionFilters;
using TestersManager.UI.Filters.AuthorizationFilter;
using TestersManager.UI.Filters.ResourceFilters;
using TestersManager.UI.Filters.ResultFilters;

namespace TestersManager.UI.Controllers;

[Route("[controller]")]
[ResponseHeaderFilterFactory("X-Custom-Key-Controller", "X-Custom-Value-Controller", 3)]
[TypeFilter(typeof(TestersAlwaysRunResultFilter))]
public class TestersController : Controller
{
    private readonly IDevStreamsGetterService _devStreamsGetterService;
    private readonly IDevStreamsAdderService _devStreamsAdderService;
    private readonly ILogger<TestersController> _logger;
    private readonly ITestersAdderService _testersAdderService;
    private readonly ITestersDeleterService _testersDeleterService;
    private readonly ITestersGetterService _testersGetterService;
    private readonly ITestersSorterService _testersSorterService;
    private readonly ITestersUpdaterService _testersUpdaterService;


    public TestersController(
        ILogger<TestersController> logger,
        ITestersGetterService testersGetterService,
        ITestersAdderService testersAdderService,
        ITestersSorterService testersSorterService,
        ITestersDeleterService testersDeleterService,
        ITestersUpdaterService testersUpdaterService, IDevStreamsGetterService devStreamsGetterService, 
        IDevStreamsAdderService devStreamsAdderService)
    {
        _logger = logger;
        _testersGetterService = testersGetterService;
        _testersAdderService = testersAdderService;
        _testersSorterService = testersSorterService;
        _testersDeleterService = testersDeleterService;
        _testersUpdaterService = testersUpdaterService;
        _devStreamsGetterService = devStreamsGetterService;
        _devStreamsAdderService = devStreamsAdderService;
    }

    [Route("[action]")]
    [Route("/")]
    [TypeFilter(typeof(TestersListActionFilter), Order = 4)]
    [ResponseHeaderFilterFactory("My-Action-Header-Key", "My-Action-Header-Value", 2)]
    [TypeFilter(typeof(TestersListResultFilter))]
    [SkipFilter]
    public async Task<IActionResult> Index(
        string searchBy,
        string? searchString,
        string sortBy = nameof(TesterResponse.TesterName),
        SortOrderOptions sortOrder = SortOrderOptions.Asc)
    {
        _logger.LogInformation("Index action method of TestersController invoked");
        _logger.LogDebug($"Search by: {searchBy}"
                         + $"\nSearch string: {searchString}"
                         + $"\nSort by: {sortBy}"
                         + $"\nSort order: {sortOrder}");

        var testers = await _testersGetterService.GetFilteredTesters(searchBy, searchString);
        var sortedTesters = await _testersSorterService.GetSortedTesters(testers, sortBy, sortOrder);

        return View(sortedTesters);
    }


    // triggers on click create
    [HttpGet]
    [Route("[action]")]
    [ResponseHeaderFilterFactory("X-Custom-Key-Action", "X-Custom-Value-Action", 4)]
    public async Task<IActionResult> Create()
    {
        var devStreams = await _devStreamsGetterService.GetAllDevStreams();
        ViewBag.DevStreams = devStreams;

        ViewBag.devStreams = devStreams.Select(x => new SelectListItem
        {
            Text = x.DevStreamName,
            Value = x.DevStreamId.ToString()
        });

        return View();
    }

    // accepts submitted form
    [HttpPost]
    [Route("[action]")]
    [TypeFilter(typeof(TesterCreateAndEditActionFilter))]
    [TypeFilter(typeof(FeatureDisableResourceFilter), Arguments = new object[] { false })]
    public async Task<IActionResult> Create(TesterAddRequest tester)
    {
        var testerResponse = await _testersAdderService.AddTester(tester);

        return RedirectToAction("Index", "Testers");
    }

    [HttpGet]
    [Route("[action]/{testerId}")] // testers/1
    [TypeFilter(typeof(TokenResultFilter))]
    public async Task<IActionResult> Edit(Guid testerId)
    {
        var testerResponse = await _testersGetterService.GetTesterById(testerId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");

        var testerUpdateRequest = testerResponse.ToTesterUpdateRequest();

        var devStreams = await _devStreamsGetterService.GetAllDevStreams();
        ViewBag.DevStreams = devStreams;

        ViewBag.devStreams = devStreams.Select(x => new SelectListItem
        {
            Text = x.DevStreamName,
            Value = x.DevStreamId.ToString()
        });

        return View(testerUpdateRequest);
    }

    [HttpPost]
    [Route("[action]/{testerId}")] // testers/1
    [TypeFilter(typeof(TesterCreateAndEditActionFilter))]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    public async Task<IActionResult> Edit(TesterUpdateRequest tester)
    {
        var testerResponse = await _testersGetterService.GetTesterById(tester.TesterId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");

        var updatedTester = await _testersUpdaterService.UpdateTester(tester);
        return RedirectToAction("Index", "Testers");
    }

    [HttpGet]
    [Route("[action]/{testerId}")]
    public async Task<IActionResult> Delete(Guid? testerId)
    {
        var testerResponse = await _testersGetterService.GetTesterById(testerId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");

        return View(testerResponse);
    }

    [HttpPost]
    [Route("[action]/{testerId}")]
    public async Task<IActionResult> Delete(TesterUpdateRequest testerUpdateRequest)
    {
        var testerResponse = await _testersGetterService.GetTesterById(testerUpdateRequest.TesterId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");

        await _testersDeleterService.DeleteTester(testerUpdateRequest.TesterId);
        return RedirectToAction("Index", "Testers");
    }

    [Route("testers-csv")]
    public async Task<IActionResult> TestersCsv()
    {
        var testers = await _testersGetterService.GetTestersCsv();

        return File(testers, "application/octet-stream", "testers.csv");
    }
}
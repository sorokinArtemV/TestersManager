using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using TestersViewer.Filters.ActionFilters;

namespace TestersViewer.Controllers;

[Route("[controller]")]
[TypeFilter(typeof(ResponseHeaderActionFilter),
    Arguments = ["X-Custom-Key-Controller", "X-Custom-Value-Controller", 3], Order = 3)]
public class TestersController : Controller
{
    private readonly IDevStreamsService _devStreamsService;
    private readonly ILogger<TestersController> _logger;
    private readonly ITestersService _testersService;


    public TestersController(
        ITestersService testersService,
        IDevStreamsService devStreamsService,
        ILogger<TestersController> logger)
    {
        _testersService = testersService;
        _devStreamsService = devStreamsService;
        _logger = logger;
    }

    [Route("[action]")]
    [Route("/")]
    [TypeFilter(typeof(TestersListActionFilter), Order = 4)]
    [TypeFilter(typeof(ResponseHeaderActionFilter),
        Arguments = ["X-Other-Key-Action", "X-Other-Value-Action", 1], Order = 1)]
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

        var testers = await _testersService.GetFilteredTesters(searchBy, searchString);
        var sortedTesters = await _testersService.GetSortedTesters(testers, sortBy, sortOrder);

        return View(sortedTesters);
    }


    // triggers on click create
    [HttpGet]
    [Route("[action]")]
    [TypeFilter(typeof(ResponseHeaderActionFilter),
        Arguments = ["X-Custom-Key-Action", "X-Custom-Value-Action", 4],
        Order = 4)]
    public async Task<IActionResult> Create()
    {
        var devStreams = await _devStreamsService.GetAllDevStreams();
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
    public async Task<IActionResult> Create(TesterAddRequest tester)
    {
        var testerResponse = await _testersService.AddTester(tester);

        return RedirectToAction("Index", "Testers");
    }

    [HttpGet]
    [Route("[action]/{testerId}")] // testers/1
    public async Task<IActionResult> Edit(Guid testerId)
    {
        var testerResponse = await _testersService.GetTesterById(testerId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");

        var testerUpdateRequest = testerResponse.ToTesterUpdateRequest();

        var devStreams = await _devStreamsService.GetAllDevStreams();
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
    public async Task<IActionResult> Edit(TesterUpdateRequest tester)
    {
        var testerResponse = await _testersService.GetTesterById(tester.TesterId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");
        
        var updatedTester = await _testersService.UpdateTester(tester);
        return RedirectToAction("Index", "Testers");
    }

    [HttpGet]
    [Route("[action]/{testerId}")]
    public async Task<IActionResult> Delete(Guid? testerId)
    {
        var testerResponse = await _testersService.GetTesterById(testerId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");

        return View(testerResponse);
    }

    [HttpPost]
    [Route("[action]/{testerId}")]
    public async Task<IActionResult> Delete(TesterUpdateRequest testerUpdateRequest)
    {
        var testerResponse = await _testersService.GetTesterById(testerUpdateRequest.TesterId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");

        await _testersService.DeleteTester(testerUpdateRequest.TesterId);
        return RedirectToAction("Index", "Testers");
    }

    [Route("testers-csv")]
    public async Task<IActionResult> TestersCsv()
    {
        var testers = await _testersService.GetTestersCsv();

        return File(testers, "application/octet-stream", "testers.csv");
    }
}
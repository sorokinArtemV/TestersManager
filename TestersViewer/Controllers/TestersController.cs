using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace TestersViewer.Controllers;

[Route("[controller]")]
public class TestersController : Controller
{
    private readonly IDevStreamsService _devStreamsService;
    private readonly ITestersService _testersService;

    public TestersController(ITestersService testersService, IDevStreamsService devStreamsService)
    {
        _testersService = testersService;
        _devStreamsService = devStreamsService;
    }

    [Route("[action]")]
    [Route("/")]
    public async Task<IActionResult> Index(
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

        var testers = await _testersService.GetFilteredTesters(searchBy, searchString);

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        // Sort
        var sortedTesters = await _testersService.GetSortedTesters(testers, sortBy, sortOrder);

        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder.ToString();

        return View(sortedTesters);
    }


    // triggers on click create
    [HttpGet]
    [Route("[action]")]
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
    public async Task<IActionResult> Create(TesterAddRequest tester)
    {
        if (!ModelState.IsValid)
        {
            var devStreams = await _devStreamsService.GetAllDevStreams();
            ViewBag.DevStreams = devStreams;

            ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToList();

            return View();
        }

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
    public async Task<IActionResult> Edit(TesterUpdateRequest testerUpdateRequest)
    {
        var testerResponse = await _testersService.GetTesterById(testerUpdateRequest.TesterId);
        if (testerResponse is null) return RedirectToAction("Index", "Testers");

        if (ModelState.IsValid)
        {
            var updatedTester = await _testersService.UpdateTester(testerUpdateRequest);
            return RedirectToAction("Index", "Testers");
        }

        var devStreams = await _devStreamsService.GetAllDevStreams();
        ViewBag.DevStreams = devStreams;

        ViewBag.devStreams = devStreams.Select(x => new SelectListItem
        {
            Text = x.DevStreamName,
            Value = x.DevStreamId.ToString()
        });

        return View(testerResponse.ToTesterUpdateRequest());
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

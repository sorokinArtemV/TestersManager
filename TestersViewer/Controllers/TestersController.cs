using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace TestersViewer.Controllers;

public class TestersController : Controller
{
    private readonly ITestersService _testersService;

    public TestersController(ITestersService testersService)
    {
        _testersService = testersService;
    }

    [Route("testers/index")]
    [Route("/")]
    public IActionResult Index(string searchBy, string? searchString)
    {
        ViewBag.SearchFields = new Dictionary<string, string>
        {
            [nameof(TesterResponse.TesterName)] = "Name",
            [nameof(TesterResponse.DevStream)] = "Stream",
            [nameof(TesterResponse.Position)] = "Position",
            [nameof(TesterResponse.Skills)] = "Skills",
            [nameof(TesterResponse.Age)] = "Age",
            [nameof(TesterResponse.Email)] = "Email",
            [nameof(TesterResponse.Gender)] = "Gender",
            [nameof(TesterResponse.MonthsOfWorkExperience)] = "Work experience"
        };

        var testers = _testersService.GetFilteredTesters(searchBy, searchString);

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        return View(testers);
    }
}
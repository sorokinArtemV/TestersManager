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
    public IActionResult Index()
    {
        ViewBag.SearchFields = new Dictionary<string, string>
        {
            [nameof(TesterResponse.TesterName)] = "Name",
            [nameof(TesterResponse.DevStream)] = "Stream",
            [nameof(TesterResponse.Position)] = "Position",
            [nameof(TesterResponse.Skills)] = "Skills",
            [nameof(TesterResponse.Email)] = "Email",
            [nameof(TesterResponse.Gender)] = "Gender",
            [nameof(TesterResponse.MonthsOfWorkExperience)] = "Work experience"
        };

        var testers = _testersService.GetAllTesters();

        return View(testers);
    }
}
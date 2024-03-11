using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

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
        var testers = _testersService.GetAllTesters();

        return View(testers);
    }
}
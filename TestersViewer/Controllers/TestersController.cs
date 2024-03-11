using Microsoft.AspNetCore.Mvc;

namespace TestersViewer.Controllers;

public class TestersController : Controller
{
    [Route("testers/index")]
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
}
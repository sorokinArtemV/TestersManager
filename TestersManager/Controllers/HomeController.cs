using Microsoft.AspNetCore.Mvc;

namespace TestersViewer.Controllers;

public class HomeController : Controller
{
    [Route("Error")]
    public IActionResult Error()
    {
        return View();
    }
}
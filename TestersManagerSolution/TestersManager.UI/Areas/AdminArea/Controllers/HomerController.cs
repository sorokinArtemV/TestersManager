using Microsoft.AspNetCore.Mvc;

namespace TestersManager.UI.Areas.AdminArea.Controllers;

public class HomerController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
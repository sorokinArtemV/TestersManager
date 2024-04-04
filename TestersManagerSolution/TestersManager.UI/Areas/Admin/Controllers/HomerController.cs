using Microsoft.AspNetCore.Mvc;

namespace TestersManager.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
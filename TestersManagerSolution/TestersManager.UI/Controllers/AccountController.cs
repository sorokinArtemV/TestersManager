using Microsoft.AspNetCore.Mvc;
using TestersManager.Core.DTO;

namespace TestersManager.UI.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterDto registerDto)
    {
        return RedirectToAction(nameof(TestersController.Index), "Testers");
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestersManager.Core.Domain.IdentityEntities;
using TestersManager.Core.DTO;

namespace TestersManager.UI.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return View(registerDto);
        }

        ApplicationUser user = new()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            TesterName = registerDto.TesterName
        };

        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded) return RedirectToAction(nameof(TestersController.Index), "Testers");


        foreach (var error in result.Errors) ModelState.AddModelError("Register", error.Description);

        return View(registerDto);
    }
}
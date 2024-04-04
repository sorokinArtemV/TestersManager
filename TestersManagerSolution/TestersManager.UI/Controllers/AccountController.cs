using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestersManager.Core.Domain.IdentityEntities;
using TestersManager.Core.DTO;
using TestersManager.Core.Enums;

namespace TestersManager.UI.Controllers;

[Route("[controller]/[action]")]
// [AllowAnonymous]
public class AccountController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;


    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }


    [HttpGet]
    [Authorize("NotAuthorized")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Authorize("NotAuthorized")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // TODO does not work for password
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

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            if (registerDto.UserType == UserTypeOptions.Admin)
            {
                if (await _roleManager.FindByNameAsync("Admin") is null)
                {
                    var applicationRole = new ApplicationRole
                    {
                        Name = UserTypeOptions.Admin.ToString()
                    };

                    await _roleManager.CreateAsync(applicationRole);
                }

                await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
            }
            else
            {
                if (registerDto.UserType == UserTypeOptions.User)
                {
                    var applicationRole = new ApplicationRole
                    {
                        Name = UserTypeOptions.User.ToString()
                    };

                    await _roleManager.CreateAsync(applicationRole);
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }
            }

            // Sign in
            await _signInManager.SignInAsync(user, false); // can create checkbox in front for this

            return RedirectToAction(nameof(TestersController.Index), "Testers");
        }

        foreach (var error in result.Errors) ModelState.AddModelError("Register", error.Description);

        // temp solution
        ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

        return View(registerDto);
    }

    [HttpGet]
    [Authorize("NotAuthorized")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Authorize("NotAuthorized")]
    public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl)
    {
        // TODO does not work
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return View(loginDto);
        }

        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

        if (result.Succeeded)
        {
            var neededUser = await _userManager.FindByEmailAsync(loginDto.Email);
            if (neededUser is not null)
                if (await _userManager.IsInRoleAsync(neededUser, UserTypeOptions.Admin.ToString()))
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return LocalRedirect(returnUrl);
            return RedirectToAction(nameof(TestersController.Index), "Testers");
        }

        ModelState.AddModelError("Login", "Invalid email or password");

        // temp solution
        ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

        return View(loginDto);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(nameof(TestersController.Index), "Testers");
    }

    [AllowAnonymous]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user is null ? Json(true) : Json(false);
    }
}
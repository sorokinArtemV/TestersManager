using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TestersViewer.Controllers;

public class HomeController : Controller
{
    [Route("Error")]
    public IActionResult Error()
    {
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (exceptionHandlerFeature?.Error != null) ViewBag.ErrorMessage = exceptionHandlerFeature.Error.Message;

        return View();
    }
}
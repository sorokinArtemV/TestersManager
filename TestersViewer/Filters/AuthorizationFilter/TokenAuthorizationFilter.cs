using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersViewer.Filters.AuthorizationFilter;

public class TokenAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Request.Cookies.ContainsKey("Auth-Key") == false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
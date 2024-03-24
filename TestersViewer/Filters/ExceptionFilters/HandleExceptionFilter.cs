using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestersViewer.Filters.ExceptionFilters;

public class HandleExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<HandleExceptionFilter> _logger;


    public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }


    public void OnException(ExceptionContext context)
    {
        _logger.LogError("Exception filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}",
            nameof(HandleExceptionFilter),
            nameof(OnException),
            context.Exception.GetType().Name,
            context.Exception.Message
        );

        if (_hostEnvironment.IsDevelopment())
            context.Result = new ContentResult
            {
                Content = context.Exception.Message,
                StatusCode = 500
            };
    }
}
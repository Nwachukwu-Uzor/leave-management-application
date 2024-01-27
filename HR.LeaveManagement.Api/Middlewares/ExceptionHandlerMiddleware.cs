using HR.LeaveManagement.Api.Models;
using HR.LeaveManagement.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HR.LeaveManagement.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        } catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        CustomProblemDetails problem = new();
        switch(ex)
        {
            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CustomProblemDetails
                {
                    Title = badRequestException.Message,
                    Errors = badRequestException.ValidationErrors,
                    Type = nameof(BadRequestException),
                    Detail = badRequestException.InnerException?.Message,
                    Status = (int)statusCode
                };
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                problem = new CustomProblemDetails
                {
                    Title = notFoundException.Message,
                    Type = nameof(NotFoundException),
                    Detail = notFoundException.InnerException?.Message,
                    Status = (int)statusCode
                };
                break;
            default:
                problem = new CustomProblemDetails
                {
                    Title = ex.Message,
                    Type = nameof(HttpStatusCode.InternalServerError),
                    Detail = ex.InnerException?.StackTrace,
                    Status = (int)statusCode
                };
                break;
        }
        var logMessage = JsonConvert.SerializeObject(problem);
        _logger.LogError(logMessage);
        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem);
    }
}

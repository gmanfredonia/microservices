using BuildingBase.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBase.Middlewares;

//https://stackoverflow.com/questions/70161542/custom-result-in-net-6-minimal-api
//https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8
//https://www.infoworld.com/article/3690408/how-to-handle-errors-in-minimal-apis-in-aspnet-core.html

public sealed class GlobalExceptionHandler(IHostEnvironment environment, IStringLocalizer localizer, ILogger<GlobalExceptionHandler> logger, IMappedExceptionCollection mappedExceptions) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        MappedException mappedException;
        ProblemDetails problem;       

        mappedException = mappedExceptions.FirstOrDefault(x => x.ExceptionType == exception.GetType());
        mappedException ??= new MappedException() { ExceptionType = typeof(UnauthorizedAccessException), StatusCode = StatusCodes.Status500InternalServerError, MessageKey = "messageGenericError" };

        problem = CreateProblemDetailsException(httpContext, exception, mappedException.StatusCode, mappedException.MessageKey);

        httpContext.Response.StatusCode = problem.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private ProblemDetails CreateProblemDetailsException(HttpContext httpContext, Exception exception, int statusCode, string detail)
    {
        bool includeExceptionDetails = environment.IsDevelopment() || environment.IsStaging();
        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        exception.Data.Add("TraceId", traceId);
        logger.LogError(exception, exception.Message);

        return CreateProblemDetails(exception.GetType().Name, statusCode, localizer[detail], httpContext.Request.Path, traceId, includeExceptionDetails ? exception : null);
    }
    private static ProblemDetails CreateProblemDetails(string title, int statusCode, string detail, string instance, string traceId, object data)
    {
        ProblemDetails result;

        result = new ProblemDetails()
        {
            Type = $"https://httpstatuses.io/{statusCode}",
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = instance
        };

        result.Extensions.Add("traceId", traceId);
        if (data is Exception)
            //Migliorare
            result.Extensions.Add("exceptionDetails", data.ToString());
        else if (data is Array)
            result.Extensions.Add("errors", data);

        return result;
    }
}
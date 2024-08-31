using CIDERS.Domain.Utils;

namespace CIDERS.Middleware;

public class ExceptionHandleMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            await HandleException(e, httpContext);
            throw;
        }
    }

    private static async Task HandleException(Exception e, HttpContext httpContext)
    {
        var error = (e is not Except exception) ?
            new
            {
                ErrorHttp.Error.Status,
                ErrorHttp.Error.Message,
                ErrorHttp.Error.ErrorCode,
                ErrorHttp.Error.Exception
            } : new
            {
                exception.GetError().Status,
                exception.GetError().Message,
                exception.GetError().ErrorCode,
                exception.GetError().Exception
            };
        httpContext.Response.StatusCode = error.Status;
        await httpContext.Response.WriteAsJsonAsync(error);
    }
}

public static class ExceptionHandleMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandleMiddleware>();
    }
}

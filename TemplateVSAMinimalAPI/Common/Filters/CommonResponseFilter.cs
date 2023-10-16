using Carter.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TemplateVSAMinimalAPI.Common.Exceptions;
using static TemplateVSAMinimalAPI.Common.Filters.CommonResponseFilter;

namespace TemplateVSAMinimalAPI.Common.Filters
{
    public class CommonResponseFilter : IEndpointFilter
    {
        public record CommonResponse<T> : IResult
        {
            public int StatusCode { get; }
            public string? Message { get; }
            public T? Result { get; }
            public object? Errors { get; }
            public CommonResponse(int statusCode, string message, T result, object errors)
            {
                StatusCode = statusCode;
                Message = message;
                Result = result;
                Errors = errors;
            }
            public CommonResponse(int statusCode, string? message, object? errors)
            {
                StatusCode = statusCode;
                Message = message;
                Errors = errors;
            }

            public CommonResponse(int statusCode, T result)
            {
                StatusCode = statusCode;
                Result = result;
                
            }
            public CommonResponse(int statusCode, T result, string message)
            {
                StatusCode = statusCode;
                Result = result;
                Message = message;

            }

            public async Task ExecuteAsync(HttpContext httpContext)
            {
                httpContext.Response.StatusCode = StatusCode;
                await httpContext.Response.AsJson(this);
            }
        }
       

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            try
            {
                var result = await next(context);
                return result;
            }
            catch (ResponseException ex)
            {
                context.HttpContext.Response.StatusCode = ex.StatusCode;
                return new CommonResponse<object>(ex.StatusCode, ex.Message, ex.Errors);
            }
            catch
            {
                throw;
            }

        }
    }
}

using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Text.Json;

namespace ZL.SemanticKernelDemo.Host.Extensions
{
    public static class MiddlewareExtension
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            // error handling pipeline (middleware)
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    // using static System.Net.Mime.MediaTypeNames;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    // customer exception
                    //if (exceptionHandlerPathFeature?.Error is BadContentException)
                    //{
                    //    // reset the status code
                    //    context.Response.StatusCode = (int)((BadContentException)exceptionHandlerPathFeature.Error).StatusCode;
                    //}

                    // todo: handle custom exceptions...
                    var baseException = exceptionHandlerPathFeature?.Error?.GetBaseException();

                    // logging (send error, stack track messages to Azure Application Insights)
                    logger.LogError(baseException?.Message);
                    logger.LogError(baseException?.StackTrace);


                    // return response
                    var errorResult = JsonSerializer.Serialize(new
                    {
                        error = baseException?.Message,
                        message = exceptionHandlerPathFeature?.Error?.Message
                    });

                    await context.Response.WriteAsync(errorResult);
                });
            });
        }
    }
}


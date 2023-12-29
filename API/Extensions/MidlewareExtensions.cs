using System.Text.Json;
using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace API.Extensions;

public static class MidlewareExtensions
{
    public static void ConfigureExceptionHandlerMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler(altPipelineBuilder =>
        {
            //Add a terminal middleware to the altPipelineBuilder to build an error response based on the type of thrown exception
            altPipelineBuilder.Run(async (context) =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                Log.Logger.Error("{@error}", errorFeature.Error);

                if (errorFeature != null)
                {
                    // Build Response Header
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = errorFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    // Build Response Body
                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(new { Error = errorFeature.Error.Message })
                    );
                }

            });

        });

    }
}

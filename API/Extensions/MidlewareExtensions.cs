using System.Text.Json;
using API.Errors;
using Azure;
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

                Log.Logger.Error("{@error}", errorFeature?.Error);

                if (errorFeature != null)
                {
                    // Build Response Header
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = errorFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        UnprocessableEntityException => StatusCodes.Status422UnprocessableEntity,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    // Build Response Body
                    var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    var bodyInJson = JsonSerializer.Serialize(
                        new ErrorResponse(
                            context.Response.StatusCode,
                            context.Response.StatusCode == StatusCodes.Status500InternalServerError ? "Internal server error." : errorFeature.Error.Message
                        ),
                        jsonOptions
                    );
                    await context.Response.WriteAsync(bodyInJson);
                }

            });

        });

    }
}

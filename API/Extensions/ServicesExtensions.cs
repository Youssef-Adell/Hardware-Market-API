using API.Errors;
using Core.DomainServices;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.Extensions;

public static class ServicesExtensions
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddControllers();
        //Modify the default behaviour of APIControllerAttribute and return a customized error response of the default one
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var validationErrors = actionContext.ModelState.Values
                    .Where(stateEntry => stateEntry.Errors.Count > 0)
                    .SelectMany(stateEntry => stateEntry.Errors)
                    .Select(modelError => modelError.ErrorMessage);

                var errorResponse = new ValidationErrorResponse(errors: validationErrors);

                return new BadRequestObjectResult(errorResponse);
            };
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppDb")));
        services.AddAutoMapper(typeof(MappingProfile));
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IProductsService, ProductsService>();
    }
}

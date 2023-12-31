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
        //Modify the default behaviour of APIControllerAttribute and return a customized error response instead of the default one
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var validationErrors = actionContext.ModelState.Values
                    .Where(stateEntry => stateEntry.Errors.Count > 0)
                    .SelectMany(stateEntry => stateEntry.Errors)
                    .Select(error => error.ErrorMessage);

                var response = new ValidationErrorResponse(details: validationErrors);

                return new BadRequestObjectResult(response);
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

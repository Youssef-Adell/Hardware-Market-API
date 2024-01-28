using API.Errors;
using Core.DomainServices;
using Core.Entities.OrderAggregate;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using FileSignatures;
using FileSignatures.Formats;
using Infrastructure.ExternalServices.FileService;
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
        //database
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppDb")));

        //mapper
        services.AddAutoMapper(typeof(MappingProfile));

        //logger
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

        //Pacakage used to check the actual type of an uploaded file (docs: https://github.com/neilharvey/FileSignatures)
        var recognisedFormats = FileFormatLocator.GetFormats().OfType<Image>();
        services.AddSingleton<IFileFormatInspector>(new FileFormatInspector(recognisedFormats));
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IBrandsRepository, BrandsRepository>();
        services.AddScoped<IProductReviewsRepository, ProductReviewsRepository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<ICouponsRepository, CouponsRepository>();

        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IBrandsService, BrandsService>();
        services.AddScoped<IProductReviewsService, ProductReviewsService>();
        services.AddScoped<IOrdersService, OrdersService>();

        services.AddScoped<IFileService, DiskFileService>();
    }
}

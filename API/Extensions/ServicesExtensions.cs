using Core.Interfaces;
using Infrastructure.Mapper;
using Infrastructure.Persistence.AppData;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ServicesExtensions
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(configuration.GetConnectionString("AppDb")));
        services.AddAutoMapper(typeof(MappingProfile));
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductsService, ProductsService>();
    }
}

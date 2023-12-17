using Core.DomainServices;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.EFConfig;
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
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IProductsService, ProductsService>();
    }
}

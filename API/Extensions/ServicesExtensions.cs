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

    }

    public static void AddApplicationServices(this IServiceCollection services)
    {

    }
}

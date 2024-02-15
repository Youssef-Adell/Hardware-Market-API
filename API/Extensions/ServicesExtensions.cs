using System.Text;
using System.Text.Json.Serialization;
using API.Errors;
using Core.DomainServices;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using FileSignatures;
using FileSignatures.Formats;
using Infrastructure.ExternalServices.AuthService;
using Infrastructure.ExternalServices.AuthService.EFConfig;
using Infrastructure.ExternalServices.EmailService;
using Infrastructure.ExternalServices.FileService;
using Infrastructure.ExternalServices.PaymentService;
using Infrastructure.Repositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace API.Extensions;

public static class ServicesExtensions
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddControllers()
                .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

        //Modify the default behaviour of APIControllerAttribute and return a customized error response instead of the default one
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var validationErrors = actionContext.ModelState.Values
                    .Where(stateEntry => stateEntry.Errors.Count > 0)
                    .SelectMany(stateEntry => stateEntry.Errors)
                    .Select(error => error.ErrorMessage);

                var response = new ErrorResponse(errors: validationErrors);

                return new BadRequestObjectResult(response);
            };
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //---app database---
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppDb")));

        //---mapper---
        services.AddAutoMapper(typeof(MappingProfile));

        //---logger---
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

        //---Pacakage used to check the actual type of an uploaded file (docs: https://github.com/neilharvey/FileSignatures)---
        var recognisedFormats = FileFormatLocator.GetFormats().OfType<Image>();
        services.AddSingleton<IFileFormatInspector>(new FileFormatInspector(recognisedFormats));

        //---identity---
        services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AuthDb")));
        // Identity (Add and configure Services To Manage, Create and Validate Users)
        services.AddIdentity<Account, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

        // configure the expiration time for generated email confirmation, phone confirmation and reset password tokens.
        services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(1));

        // Authentication (Add and configure Services required by Authentication middleware To Validate the Token came with the request)
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")))
            };
        });

    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IProductReviewsRepository, ProductReviewsRepository>();
        services.AddScoped<IBrandsRepository, BrandsRepository>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<ICouponsRepository, CouponsRepository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IBrandsService, BrandsService>();
        services.AddScoped<IProductReviewsService, ProductReviewsService>();
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddScoped<ICouponsService, CouponsService>();

        services.AddScoped<IFileService, DiskFileService>();
        services.AddScoped<IPaymentService, StripePaymentService>();
        services.AddScoped<IAuthService, IdentityAuthService>();
        services.AddScoped<IEmailService, BrevoEmailService>();
    }
}

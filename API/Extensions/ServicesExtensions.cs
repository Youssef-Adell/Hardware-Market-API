using System.Reflection;
using System.Security.Cryptography.Xml;
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
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

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
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Tech Market API",
                Description = "Rest API for Tech Market E-Commerce app.",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Youssef Adel",
                    Url = new Uri("https://www.linkedin.com/in/youssef-adell/"),
                    Email = "YoussefAdel.Fci@gmail.com"
                },
            });

            // makes Swagger-UI renders the "Authorize" button which when clicked brings up the Authorize dialog box
            options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            //comment this code because it adds the lock symbol and send the authorizatuib header to all the endpoints regardless if they marked with [authorize] or not
            // //to add lock symbol to the endpoints and send authorizatuin header with the request
            // options.AddSecurityRequirement(new OpenApiSecurityRequirement
            // {
            //     {
            //         new OpenApiSecurityScheme
            //         {
            //             Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            //         },
            //         Array.Empty<string>()
            //     }
            // });

            //to remove the lock symbol from the endpoints that doesnt has [authorize] attribute
            options.OperationFilter<SecurityRequirementsOperationFilter>(false, "bearerAuth"); // SecurityRequirementsOperationFilter has a constructor that accepts securitySchemaName and i can pass arguments to it via the OperationFilter method, so i passed the name of the SecurityScheme defined above (bearerAuth) otherwise it wont send the authorization header with the requests

            //to make swagger shows the the docs comments and responses for the endpoints 
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

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

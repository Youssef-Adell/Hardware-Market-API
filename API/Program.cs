using API.Extensions;
using Infrastructure.Repositories.EFConfig;
using Infrastructure.ExternalServices.AuthService.EFConfig;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces.IExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebServices();
builder.Services.AddInfrastructureServices(configuration: builder.Configuration);
builder.Services.AddApplicationServices();



var app = builder.Build();

// Apply Migrations that hasn't been applied and seed app and identity data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var identityDbContext = services.GetRequiredService<IdentityDbContext>();
        var authService = services.GetRequiredService<IAuthService>();
        await identityDbContext.Database.MigrateAsync();
        await identityDbContext.SeedAsync(authService);

        var appDbContext = services.GetRequiredService<AppDbContext>();
        await appDbContext.Database.MigrateAsync();
        await appDbContext.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex.Message);
    }
}

// Configure the HTTP request pipeline.
app.ConfigureExceptionHandlerMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        //To serve the SwaggerUI at the app's root (https://localhost:<port>/)
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    }
    );
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication(); // Validates the Token came at the request's Authorization header then decode it and assign it to HttpContext.User

app.UseAuthorization();

app.MapControllers();

app.Run();

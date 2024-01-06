using API.Extensions;
using Infrastructure.Repositories.EFConfig;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebServices();
builder.Services.AddInfrastructureServices(configuration: builder.Configuration);
builder.Services.AddApplicationServices();



var app = builder.Build();

// Apply Migrations that hasn't been applied and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        await dbContext.SeedAsync();
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
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

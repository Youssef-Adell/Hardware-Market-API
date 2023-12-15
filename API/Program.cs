using API.Extensions;
using Infrastructure.Persistence.AppData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebServices();
builder.Services.AddInfrastructureServices(configuration: builder.Configuration);
builder.Services.AddApplicationServices();


var app = builder.Build();

//apply migrations that hasnt been applied
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        await AppDbContextSeed.SeedAsync(dbContext);
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message);
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

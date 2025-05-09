using LoginManager.Data;
using Microsoft.EntityFrameworkCore;

namespace LoginManager.Configuration;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        
        services.AddOpenApi();
        
        return services;
    }

    public static WebApplication UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
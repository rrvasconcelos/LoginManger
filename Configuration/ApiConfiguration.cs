using LoginManager.Configuration.Options;
using LoginManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LoginManager.Configuration;

public static class ApiConfiguration
{
    private static string CorsName { get; set; } = string.Empty;

    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiOptions>(options => {
            options.FrontendUrl = configuration["FrontendUrl"] ?? string.Empty;
            options.CorsName = "AllowFrontend";
        });
        
        var apiOptions = services.BuildServiceProvider().GetRequiredService<IOptions<ApiOptions>>().Value;

        CorsName = apiOptions.CorsName;
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


        services.AddOpenApi();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsName, builder =>
            {
                builder.WithOrigins(apiOptions.FrontendUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        return services;
    }

    public static WebApplication UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseCors(CorsName);

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
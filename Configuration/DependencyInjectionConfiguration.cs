using LoginManager.Services;

namespace LoginManager.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddTransient<IAuthIdentityService, AuthIdentityService>();

        return services;
    }
}
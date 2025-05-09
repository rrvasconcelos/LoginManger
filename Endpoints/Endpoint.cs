using LoginManager.Endpoints.Identity;
using LoginManager.Endpoints.User;

namespace LoginManager.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app
            .MapGroup("");
        
        endpoints.MapGroup("/")
            .WithTags("Health Check")
            .MapGet("/", () => new { message = "OK" });

        endpoints.MapGroup("/v1/identity")
            .WithTags("Identity")
            .MapEndpoint<RegisterEndPoint>()
            .MapEndpoint<SignInEndPoint>();

        endpoints.MapGroup("/v1/user")
            .WithTags("User")
            .RequireAuthorization()
            .MapEndpoint<GetAllUserEndPoint>();
    }
    
    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        
        return app;
    }
}
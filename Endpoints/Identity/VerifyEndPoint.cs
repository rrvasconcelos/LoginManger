namespace LoginManager.Endpoints.Identity;

public class VerifyEndPoint : IEndpoint
{
    private sealed record AuthStatus(bool IsAuthenticated);
    
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/verify", HandlerAsync)
            .WithName("Verify")
            .WithTags("Identity")
            .RequireAuthorization()
            .Produces<AuthStatus>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

    private static IResult HandlerAsync(HttpContext httpContext)
    {
        return Results.Ok(new AuthStatus(true));
    }
}
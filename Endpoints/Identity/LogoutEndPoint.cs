using LoginManager.Services;

namespace LoginManager.Endpoints.Identity;

public class LogoutEndpoint : IEndpoint
{
    private sealed record LogountRequest(string Message);

    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/logout", HandlerAsync)
            .WithName("Logout")
            .WithTags("Identity");

    private static async Task<IResult> HandlerAsync(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete("auth_token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });

        await Task.CompletedTask;

        return Results.Ok(new LogountRequest("Logout realizado com sucesso"));
    }
}
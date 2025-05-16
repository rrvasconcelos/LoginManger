using LoginManager.Services;

namespace LoginManager.Endpoints.Identity;

public class SignInEndPoint : IEndpoint
{
    private sealed record SingInRequest(
        string Email,
        string Password);

    private sealed record SignInResponse(string Token);

    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/signin", HandlerAsync)
            .WithName("SignIn")
            .Produces<SignInResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Identity");

    private static async Task<IResult> HandlerAsync(
        SingInRequest request,
        ITokenService tokenService,
        IAuthIdentityService service,
        HttpContext httpContext)
    {
        var user = new Models.User()
        {
            Email = request.Email,
            UserName = request.Email
        };

        var result = await service.SignInUserAsync(user, request.Password);

        if (!result.IsSuccess)
        {
            var error = result.Error;
            return Results.Problem(
                detail: error.Description,
                title: error.Code,
                statusCode: StatusCodes.Status400BadRequest);
        }

        var resultRoles = await service.GetRolesAsync(user);

        var authResult = tokenService.GenerateToken(user, resultRoles.Value);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Previne acesso via JavaScript (essencial)
            Secure = false, // Requer HTTPS
            SameSite = SameSiteMode.Strict, // Proteção contra CSRF
            Expires = authResult.Expires, // Usa a mesma expiração do token
            Path = "/", // Disponível em toda a aplicação
            IsEssential = true // Cookie essencial (GDPR)
        };

        httpContext.Response.Cookies.Append("auth_token", authResult.Token, cookieOptions);

        return Results.Ok(new
        {
            user = request.Email,
            message = "Autenticação realizada com sucesso",
            success = true
        });
    }
}
using LoginManager.Models;
using LoginManager.Services;

namespace LoginManager.Endpoints.Identity;

public class SignInEndPoint: IEndpoint
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
    
    private static async Task<IResult> HandlerAsync(SingInRequest request, ITokenService tokenService, IAuthIdentityService service)
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
        
        var token = tokenService.GenerateToken(user, resultRoles.Value);
         
        return Results.Ok(new SignInResponse(token));
    }
}
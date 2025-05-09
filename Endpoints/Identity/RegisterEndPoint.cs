using LoginManager.Models;
using LoginManager.Services;

namespace LoginManager.Endpoints.Identity;

public sealed class RegisterEndPoint: IEndpoint
{
    private sealed record RegisterRequest( 
        string Email,
        string Document,
        string Password,
        string ConfirmPassword);

    private sealed record RegisterResponse(
        string Id,
        string Email,
        string Document);

    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/register", HandlerAsync)
            .WithName("Register")
            .Produces<RegisterResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Identity");

    private static async Task<IResult> HandlerAsync(RegisterRequest request, IAuthIdentityService service)
    {
        var user = new Models.User()
        {
            Email = request.Email,
            UserName = request.Email,
            Document = request.Document
        };
        
        var result = await service.CreateUserAsync(user, request.Password);

        if (result.IsSuccess)
            return Results.Created(
                $"/v1/register/{user.Id}",
                new RegisterResponse(user.Id, user.Email, user.Document));
        
        var error = result.Error;
        return Results.Problem(
            detail: error.Description,
            title: error.Code,
            statusCode: StatusCodes.Status400BadRequest);
    }
}
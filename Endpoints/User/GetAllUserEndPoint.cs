using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginManager.Endpoints.User;

public class GetAllUserEndPoint : IEndpoint
{
    private sealed record UserResponse(
        string Id,
        string Email,
        string Document);
    
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandlerAsync)
            .WithName("GetAllUsers")
            .Produces<List<UserResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

    private static async Task<IResult> HandlerAsync(UserManager<Models.User> userManager)
    {
        var users = await userManager.Users
            .Select(user => new UserResponse(
                user.Id,
                user.Email ?? string.Empty,
                user.Document))
            .ToListAsync();

        return users.Count == 0 ? Results.NotFound("No users found.") : Results.Ok(users);
    }
}
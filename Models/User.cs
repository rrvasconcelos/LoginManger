using Microsoft.AspNetCore.Identity;

namespace LoginManager.Models;

public class User: IdentityUser
{
    public string Document { get; init; } = string.Empty;
}
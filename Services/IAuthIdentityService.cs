using LoginManager.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginManager.Services;

public interface IAuthIdentityService
{
    Task<Result<IdentityResult>> CreateUserAsync(User user, string password);
    Task<Result<SignInResult>> SignInUserAsync(User user, string password);
    Task<Result<IList<string>>> GetRolesAsync(User user);
}

public class AuthIdentityService(UserManager<User> userManager, SignInManager<User> signInManager)
    : IAuthIdentityService
{
    public async Task<Result<IdentityResult>> CreateUserAsync(User user, string password)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (string.IsNullOrWhiteSpace(password))
            return Result<IdentityResult>.Failure(
                new Error(
                    "AuthIdentityService.CreateUserAsync",
                    "Password cannot be empty or whitespace."));

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return Result<IdentityResult>.Failure(
                new Error(
                    "AuthIdentityService.CreateUserAsync",
                    $"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}"));

        return Result<IdentityResult>.Success(result);
    }

    public async Task<Result<SignInResult>> SignInUserAsync(User user, string password)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (string.IsNullOrWhiteSpace(password))
            return Result<SignInResult>.Failure(
                new Error(
                    "AuthIdentityService.SignInUserAsync",
                    "Password cannot be empty or whitespace."));

        if (string.IsNullOrWhiteSpace(user.Email))
            return Result<SignInResult>.Failure(
                new Error(
                    "AuthIdentityService.SignInUserAsync",
                    "Email cannot be empty or whitespace."));

        var userIdentity = await userManager.FindByEmailAsync(user.Email);

        if (userIdentity is null)
            return Result<SignInResult>.Failure(
                new Error(
                    "AuthIdentityService.SignInUserAsync",
                    "User not found."));

        var result = await signInManager.PasswordSignInAsync(userIdentity, password, false, false);

        if (!result.Succeeded)
            return Result<SignInResult>.Failure(
                new Error(
                    "AuthIdentityService.SignInUserAsync",
                    "Failed to sign in user."));

        return Result<SignInResult>.Success(result);
    }

    public async Task<Result<IList<string>>> GetRolesAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var roles = await userManager.GetRolesAsync(user);

        return Result<IList<string>>.Success(roles);
    }
}
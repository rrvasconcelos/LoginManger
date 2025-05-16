namespace LoginManager.Configuration.Options;

public class JwtOptions
{
    public const string Position = "JwtSettings";
    
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 60;
    public string TokenName { get; set; } = "auth_token";
}
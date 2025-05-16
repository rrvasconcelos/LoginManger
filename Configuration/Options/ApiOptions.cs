namespace LoginManager.Configuration.Options;

public class ApiOptions
{
    public string FrontendUrl { get; set; } = string.Empty;
    public string CorsName { get; set; } = "allow-frontend";
}
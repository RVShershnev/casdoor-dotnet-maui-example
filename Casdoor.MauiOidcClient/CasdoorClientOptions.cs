namespace Casdoor.MauiOidcClient;

public class CasdoorClientOptions
{
    public CasdoorClientOptions()
    {
        Scope = "openid";
        RedirectUri = "casdoor://callback";
        Browser = new WebBrowserAuthenticator();
    }

    public string Domain { get; set; }

    public string ClientId { get; set; }

    public string RedirectUri { get; set; }

    public string Scope { get; set; }

    public IdentityModel.OidcClient.Browser.IBrowser Browser { get; set; }
}
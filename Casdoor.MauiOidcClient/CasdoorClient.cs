using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.Client;
using System.Threading;

namespace Casdoor.MauiOidcClient;

public class CasdoorClient
{
    private readonly OidcClient client;

    public CasdoorClient(CasdoorClientOptions options)
    {
        client = new OidcClient(new OidcClientOptions
        {
            Authority = $"https://{options.Domain}",
            ClientId = options.ClientId,
            Scope = options.Scope,
            RedirectUri = options.RedirectUri,
            Browser = options.Browser,
            PostLogoutRedirectUri = options.RedirectUri
        });
        client.Options.BackchannelHandler = new HttpClientHandler() 
        { 
            ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true 
        };
    }

    public IdentityModel.OidcClient.Browser.IBrowser Browser
    {
        get
        {
            return client.Options.Browser;
        }
        set
        {
            client.Options.Browser = value;
        }
    }

    public async Task<LoginResult> LoginAsync()
    {
        return await client.LoginAsync();
    }

    public async Task<LogoutResult> LogoutAsync(string acsessToken)
    {
        var logoutParameters = new Dictionary<string, string>
        {
            {"client_id", client.Options.ClientId },
            {"returnTo", client.Options.RedirectUri }
        };

        var logoutRequest = new LogoutRequest();
        logoutRequest.IdTokenHint = acsessToken;
        //var endSessionUrl = new RequestUrl($"{oidcClient.Options.Authority}/api/logout")
        //   .Create(new Parameters(logoutParameters));
        //var browserOptions = new BrowserOptions(endSessionUrl, oidcClient.Options.RedirectUri)
        //{
        //    Timeout = TimeSpan.FromMilliseconds(100),
        //    DisplayMode = logoutRequest.BrowserDisplayMode,       
        //};              

        return await client.LogoutAsync(logoutRequest);
    }
}
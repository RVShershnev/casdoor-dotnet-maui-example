using IdentityModel.OidcClient.Browser;

namespace Casdoor.MauiOidcClient;

public class WebViewBrowserAuthenticator : IdentityModel.OidcClient.Browser.IBrowser
{
    private readonly WebView webView;

    public WebViewBrowserAuthenticator(WebView webView)
    {
        this.webView = webView;
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<BrowserResult>();

        webView.Navigated += (sender, e) =>
        {
            if (e.Url.StartsWith(options.EndUrl))
            {
                webView.WidthRequest = 0;
                webView.HeightRequest = 0;
                if (tcs.Task.Status != TaskStatus.RanToCompletion)
                {
                    tcs.SetResult(new BrowserResult
                    {
                        ResultType = BrowserResultType.Success,
                        Response = e.Url.ToString()
                    });
                }
            }

        };

        webView.WidthRequest = 600;
        webView.HeightRequest = 600;
        webView.Source = new UrlWebViewSource { Url = options.StartUrl };

        return await tcs.Task;
    }
}

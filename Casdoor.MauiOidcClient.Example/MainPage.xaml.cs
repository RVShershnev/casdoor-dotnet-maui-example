using IdentityModel.OidcClient;

namespace Casdoor.MauiOidcClient.Example
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly CasdoorClient client;
        private string acsessToken;
        public MainPage(CasdoorClient client)
        {
            InitializeComponent();
            this.client = client;

#if WINDOWS
    client.Browser = new WebViewBrowserAuthenticator(WebViewInstance);
#endif
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var loginResult = await client.LoginAsync();
            acsessToken = loginResult.AccessToken;
            if (!loginResult.IsError)
            {
                NameLabel.Text = loginResult.User.Identity.Name;
                EmailLabel.Text = loginResult.User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;            

                LoginView.IsVisible = false;
                HomeView.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var logoutResult = await client.LogoutAsync(acsessToken);


            if (!logoutResult.IsError)
            {
                HomeView.IsVisible = false;
                LoginView.IsVisible = true;
                this.Focus();
            }
            else
            {
                await DisplayAlert("Error", logoutResult.ErrorDescription, "OK");
            }
        }
    }
}
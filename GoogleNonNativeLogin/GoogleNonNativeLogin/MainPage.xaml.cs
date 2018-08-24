using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.FacebookClient.Abstractions;
using System;
using System.Diagnostics;
using Xamarin.Auth;
using Xamarin.Forms;

namespace GoogleNonNativeLogin
{
    public partial class MainPage : ContentPage
    {
        private Account account;
        private FacebookUser facebookUser;
        private AccountStore store;
        private User user;
        public MainPage()
        {
            InitializeComponent();

            //"client_id": "1044151994132-1hi4qvc6te28qc1jt254o3lsq0au0uhf.apps.googleusercontent.com",
            //"project_id": "grial-213609",
            //"auth_uri": "https://accounts.google.com/o/oauth2/auth",
            //"token_uri": "https://www.googleapis.com/oauth2/v3/token",
            //"auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
            //"redirect_uris": [
            //"urn:ietf:wg:oauth:2.0:oob",
            //"http://localhost"
            //    ]
            //com.googleusercontent.apps.1044151994132-1hi4qvc6te28qc1jt254o3lsq0au0uhf
        }

        private void FBSignInbuttonPressed(object sender, EventArgs e)
        {

            loginWithFB();

        }

        private void FBSignOutnbuttonPressed(object sender, EventArgs e)
        {
            CrossFacebookClient.Current.Logout();

        }

        private void GoogleSignInbuttonPressed(object sender, EventArgs e)
        {
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);
            authenticator.Completed += OnAuthenticationCompletedAsync;
            authenticator.Error += OnAuthenticationFailed;
            AuthenticationState.Authenticator = authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            presenter.Login(authenticator);
        }

        private void GoogleSignOutbuttonPressed(object sender, EventArgs e)
        {

        }

        private async void OnAuthenticationCompletedAsync(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthenticationCompletedAsync;
                authenticator.Error -= OnAuthenticationFailed;
            }

            if (e.IsAuthenticated)
            {//now user is authenticated,so lets go and save important keys
                AccountStore.Create().Save(e.Account, Constants.AppName);

                var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    string userJson = response.GetResponseText();
                    user = JsonConvert.DeserializeObject<User>(userJson);

                    //lets update ui with data received from provider
                    UpdateProfileUIWith(user);
                }

            }
        }
        
        
        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
        }
        private void TwitterSignInbuttonPressed(object sender, EventArgs e)
        {
            loginWithTwitter();

        }

        private void loginWithTwitter()
        {
            // start login with twitter
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }

            var TwitterAuthenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);
            TwitterAuthenticator.Completed += OnTwitterAuthenticationCompletedAsync;
            TwitterAuthenticator.Error += OnTwitterAuthenticationFailed;
            AuthenticationState.Authenticator = TwitterAuthenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            presenter.Login(TwitterAuthenticator);
        }

        private void OnTwitterAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnTwitterAuthenticationCompletedAsync(object sender, AuthenticatorCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TwitterSignOutnbuttonPressed(object sender, EventArgs e)
        {

        }

        private void UpdateProfileUIWith(User user)
        {
            xProfilePicure.Source = user.Picture ;
            xProfileName.Text = user.GivenName + " " + user.FamilyName;
            xProfileEmail.Text = user.Email;
            xProfileOtherData.Text = "Gender: " + user.Gender + " FamilyName: " + user.FamilyName + " Verified: " + user.VerifiedEmail;

        }
        private void UpdateProfileUIWith(FacebookUser user)
        {
            xProfilePicure.Source = user.Picture.Data.Url;
            xProfileName.Text = user.Email;
            xProfileEmail.Text = user.ShortName;
            xProfileOtherData.Text = "Short Name : " + user.ShortName + " ID: " + user.Id;

        }

        public async void loginWithFB()
        {
            var fbLoginResponse = await CrossFacebookClient.Current.LoginAsync(new string[] { "email" });


            if (fbLoginResponse.Status == FacebookActionStatus.Completed)
            {
                var fbUserAccessToken = CrossFacebookClient.Current.ActiveToken;
                //var resp = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "picture" }, new string[] { "default" });
                var resp = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "id","email", "first_name", "last_name", "middle_name", "name", "name_format", "picture", "short_name" }, new string[] { "email" });
                Debug.WriteLine(resp.Data+ "");

                //Deserialize data into fbUser
                facebookUser = JsonConvert.DeserializeObject<FacebookUser>(resp.Data);
                await DisplayAlert("Fb data received", facebookUser.Name, "OK");
                UpdateProfileUIWith(facebookUser);

            }
            else
            {
                await DisplayAlert("Something Went Wrong", "Could not authenticate you with facebook \nTry Again", "OK");
            }
        }
    }
}
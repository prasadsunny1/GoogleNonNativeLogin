using GoogleNonNativeLogin.Models;
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
        private GoogleUserModel user;

        public MainPage()
        {
            InitializeComponent();
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

            var TwitterAuthenticator = new OAuth1Authenticator(consumerKey: "Ou4uAUPALb1YEyA06OThikxRU",
                                                                consumerSecret: "cou4ELdCtRxaKwDf1t5e5CIbePzO4Xj2pLcTIK3Zi481YzvhXK",
                                                                requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
                                                                authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
                                                                accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
                                                                callbackUrl: new Uri("https://mobile.twitter.com/o1auth/"));

            TwitterAuthenticator.Completed += OnTwitterAuthenticationCompletedAsync;
            TwitterAuthenticator.Error += OnTwitterAuthenticationFailed;

            AuthenticationState.TwitterAuthenticatorState = TwitterAuthenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            presenter.Login(TwitterAuthenticator);
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
                    user = JsonConvert.DeserializeObject<GoogleUserModel>(userJson);

                    //lets update ui with data received from provider
                    UpdateProfileUIWith(user);
                }
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
        }

        private async void OnTwitterAuthenticationCompletedAsync(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth1Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnTwitterAuthenticationCompletedAsync;
                authenticator.Error -= OnTwitterAuthenticationFailed;
            }

            if (e.IsAuthenticated)
            {
                //now user is authenticated,so lets go and save important keys
                AccountStore.Create().Save(e.Account, Constants.TwitterAppNameAndorid);

                var request = new OAuth1Request("GET", new Uri("https://api.twitter.com/1.1/account/verify_credentials.json"), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    string userJson = response.GetResponseText();
                    Debug.Print(userJson);
                    Models.TwitterUser twitterUser = JsonConvert.DeserializeObject<Models.TwitterUser>(userJson);

                    //lets update ui with data received from provider
                    UpdateProfileUIWith(twitterUser);
                }

                //var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                //var response = await request.GetResponseAsync();
                //if (response != null)
                //{
                //string userJson = response.GetResponseText();
                //user = JsonConvert.DeserializeObject<GoogleUserModel>(userJson);

                ////lets update ui with data received from provider
                //UpdateProfileUIWith(user);
                //}
            }
        }

        private void OnTwitterAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            DisplayAlert("Twitter Login Failed", "", "Close");
        }

        private void TwitterSignInbuttonPressed(object sender, EventArgs e)
        {
            loginWithTwitter();
        }
        private void TwitterSignOutnbuttonPressed(object sender, EventArgs e)
        {
        }

        private void UpdateProfileUIWith(TwitterUser twitterUser)
        {
            xProfileEmail.Text = twitterUser.ScreenName;
            xProfileName.Text = twitterUser.Name;
            xProfilePicure.Source = twitterUser.ProfileImageUrl;
            xProfileOtherData.Text = twitterUser.FollowersCount + " followers and " + twitterUser.FriendsCount + "  friends";
        }
        private void UpdateProfileUIWith(GoogleUserModel user)
        {
            xProfilePicure.Source = user.Picture;
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
                var resp = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "id", "email", "first_name", "last_name", "middle_name", "name", "name_format", "picture", "short_name" }, new string[] { "email" });
                Debug.WriteLine(resp.Data + "");

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GoogleNonNativeLogin.Droid
{
    [Activity(Label = "TwitterRedirectInterceptor", NoHistory = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    [IntentFilter(
       new[] { Intent.ActionView },
       Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
       DataScheme = "https://mobile.twitter.com/o1auth/"
       )]

    class TwitterRedirectInterceptor:Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Convert Android.Net.Url to Uri
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            AuthenticationState.TwitterAuthenticatorState.OnPageLoading(uri);
            AuthenticationState.TwitterAuthenticatorState.OnPageLoaded(uri);
            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);
            Finish();
        }
    }
}
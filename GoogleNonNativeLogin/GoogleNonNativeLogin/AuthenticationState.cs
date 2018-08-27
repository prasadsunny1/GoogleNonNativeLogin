using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;

namespace GoogleNonNativeLogin
{
    public class AuthenticationState
    {
        public static OAuth2Authenticator Authenticator;
        public static OAuth1Authenticator TwitterAuthenticatorState;
    }
}

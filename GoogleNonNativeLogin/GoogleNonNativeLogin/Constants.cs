﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleNonNativeLogin
{
    class Constants
    {

        public static string AppName = "OauthbySunnyDemo";
        // OAuth
        // For Google login, configure at https://console.developers.google.com/
        public static string iOSClientId = "1044151994132-6ctbkl2rfetlu7uf91cavp3gitk3s04o.apps.googleusercontent.com";
        public static string AndroidClientId = "1044151994132-1hi4qvc6te28qc1jt254o3lsq0au0uhf.apps.googleusercontent.com";

        // These values do not need changing
        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v3/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string iOSRedirectUrl = "com.googleusercontent.apps.1044151994132-6ctbkl2rfetlu7uf91cavp3gitk3s04o:/oauth2redirect";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.1044151994132-1hi4qvc6te28qc1jt254o3lsq0au0uhf:/oauth2redirect";

        // Twitter Constants
        public static string TwitterAppNameAndorid = "TwitterAndroidAccount";
        public static string TwitterConsumerKey = "Ou4uAUPALb1YEyA06OThikxRU";
        public static string TwitterConsumerSecret = "cou4ELdCtRxaKwDf1t5e5CIbePzO4Xj2pLcTIK3Zi481YzvhXK";
        public static string TwitterRequestTokenUrl = "https://api.twitter.com/oauth/request_token";
        public static string TwitterAuthorizeUrl = "https://api.twitter.com/oauth/authorize";
        public static string TwitterAccessTokenUrl = "https://api.twitter.com/oauth/access_token";
        public static string TwitterCallbackUrl = "https://mobile.twitter.com/o1auth/";


    }
}

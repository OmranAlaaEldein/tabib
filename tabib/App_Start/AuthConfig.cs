using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using tabib.Models;
using Owin;

namespace tabib
{
    public partial class AuthConfig
    {
        public static void RegisterAuth( )
        {

           
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

           // OAuthWebSecurity.RegisterGoogleClient();

            //clientId: "837157477078-ssa187550fbtjkgbi982i4u7ho1lenrc.apps.googleusercontent.com",
            //  clientSecret: "1fcbUQRts0uEPjxZ6BBGkJdt"  
        }
    }
}

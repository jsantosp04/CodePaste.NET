﻿using System;
using System.Security.Claims;
using System.Web.Helpers;
using CodePasteBusiness;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.GitHub;

namespace CodePasteMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/LogOn")
            });

            // MAKE SURE PROVIDERS KEYS ARE SET IN the CodePasteKeys.json FILE
            if (App.Secrets.GoogleClientId == null)
                throw new ArgumentException("External Logon Provider keys appear to be missing. Please update the values in the separate configuration file.");

            // these values are stored in CodePasteKeys.json
            // and are NOT included in repro - autocreated on first load
            if (!string.IsNullOrEmpty(App.Secrets.GoogleClientId))
            {
                app.UseGoogleAuthentication(
                    clientId: App.Secrets.GoogleClientId,
                    clientSecret: App.Secrets.GoogleClientSecret);
            }

            if (!string.IsNullOrEmpty(App.Secrets.TwitterConsumerKey))
            {
                app.UseTwitterAuthentication(
                    consumerKey: App.Secrets.TwitterConsumerKey,
                    consumerSecret: App.Secrets.TwitterConsumerSecret);
            }

            if (!string.IsNullOrEmpty(App.Secrets.GitHubClientId))
            {
                app.UseGitHubAuthentication(App.Secrets.GitHubClientId, App.Secrets.GitHubClientSecret);
            }

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}
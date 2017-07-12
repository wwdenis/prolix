// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;

using Owin;
using System;
using System.Security.Claims;

namespace Wwa.Identity.AspNet
{
    /// <summary>
    /// OWIN Identity Server Activator
    /// </summary>
    public class IdentityServer
    {
        #region Static Fields

        static OAuthBearerAuthenticationOptions _bearerOptions;
        static OAuthAuthorizationServerOptions _serverOptions;

        // TODO: Make configurable
        static string QueryStringKey = "token";
        static string DatabaseContext = "identity:DataContext";
        static int AccessFailedLimit = 3;
        static int TokenExpirationHours = 24;

        #endregion

        #region Instance Methods

        public void Configuration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(CreateDbContext);
            app.CreatePerOwinContext<UserManager<IdentityUser>>(CreateManager, DisposeManager);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            _bearerOptions = new OAuthBearerAuthenticationOptions()
            {
                Provider = new BearerTokenProvider(QueryStringKey)
            };

            _serverOptions = new OAuthAuthorizationServerOptions
            {
                Provider = new OAuthAuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(TokenExpirationHours),

                // TokenEndpointPath = new PathString("/Token"),
                // AuthorizeEndpointPath = new PathString("/Login"),
                
                // TODO: Remove on production
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(_serverOptions);
            app.UseOAuthBearerAuthentication(_bearerOptions);
        }

        #endregion

        #region Public Static Methods

        public static string IssueToken(string userId)
        {
            var identity = new ClaimsIdentity(_bearerOptions.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, userId));

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());

            var now = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = now;
            ticket.Properties.ExpiresUtc = now.Add(_serverOptions.AccessTokenExpireTimeSpan);

            var token = _bearerOptions.AccessTokenFormat.Protect(ticket);
            return token;
        }

        #endregion

        #region Private Static Methods

        static UserManager<IdentityUser> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, IOwinContext context)
        {
            var db = context.Get<IdentityDbContext>();
            var store = new UserStore<IdentityUser>(db);
            
            var manager = new UserManager<IdentityUser>(store)
            {
                UserLockoutEnabledByDefault = true,
                MaxFailedAccessAttemptsBeforeLockout = AccessFailedLimit,
                DefaultAccountLockoutTimeSpan = new DateTime(2015, 1, 1) - new DateTime(2115, 1, 1),

                // Configure validation logic for passwords
                //PasswordValidator = new PasswordValidator
                //{
                //    RequiredLength = 6,
                //    RequireNonLetterOrDigit = true,
                //    RequireDigit = true,
                //    RequireLowercase = true,
                //    RequireUppercase = true,
                //}
            };

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<IdentityUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            IDataProtector protector = options?.DataProtectionProvider?.Create();

            if (protector != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(protector);
            }
            
            return manager;
        }

        static void DisposeManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, UserManager<IdentityUser> manager)
        {
            manager?.Dispose();
        }


        static IdentityDbContext CreateDbContext()
        {
            return new IdentityDbContext(DatabaseContext);
        }
        
        #endregion
    }
}

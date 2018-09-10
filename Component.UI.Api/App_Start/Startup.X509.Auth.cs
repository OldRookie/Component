using Component.Infrastructure.Security;
using Component.UI.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Component.UI.Api.App_Start
{
    public class StartupX509
    {
        /// <summary>
        ///  the current assembly version of the website
        /// </summary>
        public static Version AssemblyVersion { get; private set; }

        /// <summary>
        /// System Connection string
        /// </summary>
        public static string ConnectionString { get; private set; }

        /// <summary>
        /// Public System Id
        /// </summary>
        public static string PublicClientId { get; private set; }

        /// <summary>
        /// CertThumbPrint
        /// </summary>
        public static string CertThumbPrint { get; private set; }

        /// <summary>
        /// PrivateKey
        /// </summary>
        public static string PrivateKey { get; private set; }

        /// <summary>
        /// PublicKey
        /// </summary>
        public static string PublicKey { get; private set; }

        /// <summary>
        /// X509Certification
        /// </summary>
        public static X509Certificate2 X509Certification { get; set; }

        /// <summary>
        /// AuthAuthorizationServerOptions
        /// </summary>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /// <summary>
        /// Token Provider
        /// </summary>
        public static JwtFormatter JsonWebTokenFormatter { get; private set; }

        static StartupX509()
        {
            //Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            AssemblyVersion = assemblyName.Version; //set the version info

            PublicClientId = "Intertek Identity Server " + AssemblyVersion.ToString();

            // Load From Config on Thumbprint
           
            //Search cert
            X509Certification = X509CertificationHelper.Search(CertThumbPrint);
            if (X509Certification == null)

            PrivateKey = X509CertificationHelper.GetPrivateKey();
            PublicKey = X509CertificationHelper.GetPublicKey(X509Certification);

            // Set Oauth
            JsonWebTokenFormatter = new JwtFormatter(X509Certification);
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new OAuthProvider(PublicClientId),
                AccessTokenFormat = JsonWebTokenFormatter,
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                AllowInsecureHttp = true,
                //RefreshTokenProvider = new RefreshTokenProvider()
            };

            // Add the Cache
            System.Web.HttpRuntime.Cache.Insert("PrivateKey", PrivateKey);
            System.Web.HttpRuntime.Cache.Insert("PublicKey", PublicKey);
            System.Web.HttpRuntime.Cache.Insert("CertThumbPrint", CertThumbPrint);
            System.Web.HttpRuntime.Cache.Insert("ConnectionString", ConnectionString);
            System.Web.HttpRuntime.Cache.Insert("VersionString", AssemblyVersion.ToString());
            // System.Web.HttpRuntime.Cache.Insert("IsValidateExternalUser", "No_Validate_External_Endpoint");
        }


        public void ConfigureAuth(IAppBuilder app)
        {
            // Support the Cors 
            //app.UseCors(CorsOptions.AllowAll);
            // Support the End point
            app.UseOAuthBearerTokens(OAuthOptions);

            // Configure the db context, user manager,role manager signin manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Support MVC Sign in
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                //LoginPath = new PathString("/Account/Login"), // set LoginPath will both redirect the MVC and API 401 unauthorize.

                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //Provider = new CookieAuthenticationProvider
                //{
                //    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                //    validateInterval: TimeSpan.FromMinutes(20),
                //    regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                //    getUserIdCallback: (id) => (int.Parse(id.GetUserId())))
                //}
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}
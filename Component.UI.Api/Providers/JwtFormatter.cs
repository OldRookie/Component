using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Component.UI.Api.Providers
{

    public class JwtFormatter : ISecureDataFormat<AuthenticationTicket>
    {
        private const string IssuerName = "LOCAL AUTHORITY";
        private readonly string AudienceName = "External Applications Implementation OAuth2";
        private readonly string _X509SecurityAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        private readonly X509Certificate2 _X509Certificate2;
        private readonly X509SecurityKey _X509SecurityKey;
        private readonly Microsoft.IdentityModel.Tokens.SigningCredentials _X509SigningCredentials;


        private TokenValidationParameters _TokenValidationParameters { get; set; }
        private JwtSecurityTokenHandler _JwtSecurityTokenHandler { get; set; }

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="x509Certificate2">Certification Object</param>
        public JwtFormatter(X509Certificate2 x509Certificate2)
        {
            this._X509Certificate2 = x509Certificate2;
            this._X509SecurityKey = new X509SecurityKey(this._X509Certificate2);
            this._X509SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(this._X509SecurityKey, this._X509SecurityAlgorithm);
            this.AudienceName = this._X509Certificate2.Thumbprint;

            this._TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = IssuerName,
                ValidAudience = AudienceName,
                ValidateLifetime = true,
                AuthenticationType = "JWT",
                IssuerSigningKey = this._X509SecurityKey
            };
            this._JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }


        /// <summary>
        ///  Protect
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Protect(AuthenticationTicket data)
        {
            DateTimeOffset? issued = data.Properties.IssuedUtc;
            DateTimeOffset? expires = data.Properties.ExpiresUtc;
            return this._JwtSecurityTokenHandler.CreateJwtSecurityToken(
                _TokenValidationParameters.ValidIssuer,
                _TokenValidationParameters.ValidAudience,
                data.Identity,
                DateTime.Now.ToUniversalTime(),
                expires.Value.UtcDateTime,
                issued.Value.UtcDateTime,
                this._X509SigningCredentials
            ).RawData;
        }

        /// <summary>
        /// Unprotect
        /// </summary>
        /// <param name="protectedText"></param>
        /// <returns></returns>
        public AuthenticationTicket Unprotect(string protectedText)
        {
            try
            {
                // Lazy argument with issuers and tokens. Note these may be refreshed periodically.
                TokenValidationParameters validationParameters = this._TokenValidationParameters.Clone();
                // Lazy argument with Validation 
                Microsoft.IdentityModel.Tokens.SecurityToken validatedToken;
                ClaimsPrincipal claimsPrincipal = _JwtSecurityTokenHandler.ValidateToken(protectedText, validationParameters, out validatedToken);
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;

                // Fill out the authenticationProperties issued and expires times if the equivalent claims are in the JWT
                var authenticationProperties = new AuthenticationProperties();
                if (this._TokenValidationParameters.ValidateLifetime)
                {
                    // Override any session persistence to match the token lifetime.
                    DateTime issued = validatedToken.ValidFrom;
                    if (issued != DateTime.MinValue)
                    {
                        authenticationProperties.IssuedUtc = issued.ToUniversalTime();
                    }
                    DateTime expires = validatedToken.ValidTo;
                    if (expires != DateTime.MinValue)
                    {
                        authenticationProperties.ExpiresUtc = expires.ToUniversalTime();
                    }
                    authenticationProperties.AllowRefresh = false;
                }

                return new AuthenticationTicket(claimsIdentity, authenticationProperties);
            }
            catch
            {
                return null;
            }

        }
    }

}
﻿using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using hr_app.EntityFramework;
using hr_app.Models;

namespace hr_app.WWW
{
    public static class AzureAdB2CAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAzureAdB2C(this AuthenticationBuilder builder)
            => builder.AddAzureAdB2C(_ =>
            {
            });

        public static AuthenticationBuilder AddAzureAdB2C(this AuthenticationBuilder builder, Action<AzureAdB2COptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, OpenIdConnectOptionsSetup>();
            //builder.Services.AddScoped<HRAppDBContext>();
            //builder.Services.AddSingleton<IUserService, UserService>();
            builder.AddOpenIdConnect();
            return builder;
        }

        public class OpenIdConnectOptionsSetup : IConfigureNamedOptions<OpenIdConnectOptions>
        {
            private readonly DataContext _context;
            private readonly IServiceScopeFactory _scopeFactory;
            public OpenIdConnectOptionsSetup(IOptions<AzureAdB2COptions> b2cOptions, IServiceScopeFactory scopeFactory)
            {
                AzureAdB2COptions = b2cOptions.Value;
                _scopeFactory = scopeFactory;
            }

            public AzureAdB2COptions AzureAdB2COptions { get; set; }

            public void Configure(string name, OpenIdConnectOptions options)
            {
                options.ClientId = AzureAdB2COptions.ClientId;
                options.Authority = AzureAdB2COptions.Authority;
                options.UseTokenLifetime = true;
                options.TokenValidationParameters = new TokenValidationParameters() { NameClaimType = "name" };
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                options.Events = new OpenIdConnectEvents()
                {
                    OnRedirectToIdentityProvider = OnRedirectToIdentityProvider,
                    OnRemoteFailure = OnRemoteFailure,
                    OnAuthorizationCodeReceived = context =>
                    {
                        return OnAuthorizationCodeReceived(context);
                    }
                };
            }

            public void Configure(OpenIdConnectOptions options)
            {
                Configure(Options.DefaultName, options);
            }

            public Task OnRedirectToIdentityProvider(RedirectContext context)
            {
                var defaultPolicy = AzureAdB2COptions.DefaultPolicy;
                if (context.Properties.Items.TryGetValue(AzureAdB2COptions.PolicyAuthenticationProperty, out var policy) &&
                    !policy.Equals(defaultPolicy))
                {
                    context.ProtocolMessage.Scope = OpenIdConnectScope.OpenIdProfile;
                    context.ProtocolMessage.ResponseType = OpenIdConnectResponseType.IdToken;
                    context.ProtocolMessage.IssuerAddress = context.ProtocolMessage.IssuerAddress.ToLower().Replace(defaultPolicy.ToLower(), policy.ToLower());
                    context.Properties.Items.Remove(AzureAdB2COptions.PolicyAuthenticationProperty);
                }
                else if (!string.IsNullOrEmpty(AzureAdB2COptions.ApiUrl))
                {
                    context.ProtocolMessage.Scope += $" offline_access {AzureAdB2COptions.ApiScopes}";
                    context.ProtocolMessage.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                }
                return Task.FromResult(0);
            }

            public Task OnRemoteFailure(RemoteFailureContext context)
            {
                context.HandleResponse();
                // Handle the error code that Azure AD B2C throws when trying to reset a password from the login page 
                // because password reset is not supported by a "sign-up or sign-in policy"
                if (context.Failure is OpenIdConnectProtocolException && context.Failure.Message.Contains("AADB2C90118"))
                {
                    // If the user clicked the reset password link, redirect to the reset password route
                    context.Response.Redirect("/Session/ResetPassword");
                }
                else if (context.Failure is OpenIdConnectProtocolException && context.Failure.Message.Contains("access_denied"))
                {
                    context.Response.Redirect("/");
                }
                else
                {
                    context.Response.Redirect("/Home/Error?message=" + Uri.EscapeDataString(context.Failure.Message));
                }
                return Task.FromResult(0);
            }

            public async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
            {
                // Use MSAL to swap the code for an access token
                // Extract the code from the response notification
                var code = context.ProtocolMessage.Code;

                string signedInUserID = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                IConfidentialClientApplication cca = ConfidentialClientApplicationBuilder.Create(AzureAdB2COptions.ClientId)
                    .WithB2CAuthority(AzureAdB2COptions.Authority)
                    .WithRedirectUri(AzureAdB2COptions.RedirectUri)
                    .WithClientSecret(AzureAdB2COptions.ClientSecret)
                    .Build();
                new MSALStaticCache(signedInUserID, context.HttpContext).EnablePersistence(cca.UserTokenCache);

                if (context.Principal != null)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<DataContext>();


                        // when we exit the using block,
                        // the IServiceScope will dispose itself 
                        // and dispose all of the services that it resolved.
                    }

                    var claims = context.Principal.Identities.First().Claims;
                    User au = new User();
                    var v = claims.FirstOrDefault(x => ClaimTypes.Email == x.Type || x.Type == "email" || x.Type == "emails");
                    var v1 = claims.FirstOrDefault(x => ClaimTypes.GivenName == x.Type);
                    var v2 = claims.FirstOrDefault(x => ClaimTypes.Surname == x.Type);
                    au.Email = v != null ? v.Value : "";
                    
                    //var first = _context.Users.Where(x => x.Email == au.Email).Include(i => i.Role).FirstOrDefault();



                    //_context.Users.Select(x=>x).Include().FirstOrDefault(x => x.Email == au.Email);

                    //if (first == null)
                    {
                        //au.Role = _context.Roles.First(x => x.RoleName == Roles.User.ToString());
                        //_context.Users.Add(au);
                        //_context.SaveChanges();
                        //first = au;
                    }
                    context.Principal.Identities.First().AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    context.Principal.Identities.First().AddClaim(new Claim(context.Principal.Identities.First().NameClaimType, "Bob"));
                    var p = context.Principal.Identities.First();
                }

                try
                {
                    AuthenticationResult result = await cca.AcquireTokenByAuthorizationCode(AzureAdB2COptions.ApiScopes.Split(' '), code)
                        .ExecuteAsync();
                    context.HandleCodeRedemption(result.AccessToken, result.IdToken);
                }
                catch
                {
                    //TODO: Handle
                    throw;
                }
            }
        }
    }
}
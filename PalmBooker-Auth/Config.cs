// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace EBookkeepingAuth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource("ebookkeeping-api", "EBookkeeping API- V1")
                {
                    ApiSecrets={ new Secret("5277A8EE-67F5-4350-8CFC-267886E549F2".Sha256()) }
                },
                new ApiResource("ebookkeeping-api2", "HAA - V1")
                {
                    ApiSecrets={ new Secret("C8817C15-BA73-41E3-B44F-D62917F92EE5".Sha256()) },
                },
                new ApiResource("ebookkeeping-api3", "HAAD - V1")
                {
                    ApiSecrets={ new Secret("03F04B9A-3D98-4E21-AE54-CA94992CD562".Sha256()) }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId= "ebookkeeping-nm",
                    ClientName= "Ebookkeeping native mobile",
                    AllowedGrantTypes= {"authorization_code"} ,
                    AllowedScopes = { "openid", "profile", "ebookkeeping-api","offline_access" },
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken= true,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent=false,
                    RedirectUris = {
                        "com.ebookkeeping.psl:/oauth2callback"
                    }
                },               
                //swagger client
                new Client
                {
                    ClientId="api-swagger",
                    Enabled = true,
                    ClientName="Swagger",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "ebookkeeping-api","web_services_api","ebookkeeping-api3"
                    },
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44363/api-docs/oauth2-redirect.html",
                        "https://psl-app-vm3/ebookkeepingapi/api-docs/oauth2-redirect.html"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:44363",
                        "https://psl-app-vm3"
                    },
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false

                },
                new Client
                {
                    ClientId="web_services_api",
                    ClientName = "Web Services Api",
                    ClientSecrets=new[]{new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.Code,
                    AllowedScopes=new[]
                    {
                        "web_services_api"
                    }
                },
                new Client
                {
                    ClientId= "xamarin-client",
                    ClientName= "Xamarin Client",
                    AllowedGrantTypes=  GrantTypes.Code,
                    AllowedScopes= new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "web_services_api"
                    },
                    AllowAccessTokensViaBrowser= true,
                    AllowOfflineAccess=true,
                    AlwaysIncludeUserClaimsInIdToken= true,
                    RequirePkce=true,
                    RequireClientSecret= false,
                    RedirectUris= new[]{
                        //"http://192.168.0.249:5000/grants"
                        "http://192.168.0.249:2000/signin-oidc"
                    }
                },
                new Client
                {

                    ClientId="ebookkeeping-pwa",
                    ClientName = "Ebookkeeping PWA",
                    AllowedGrantTypes=GrantTypes.Code,
                    RedirectUris=new[]
                    { 
                        "http://127.0.0.1:8887/signin-oidc",
                        "http://127.0.0.1:3000/signin-oidc",
                        "http://localhost:3000/signin-oidc",
                        "https://psl-app-vm3/BusinessEbookkeeping/signin-oidc" 
                    },
                    PostLogoutRedirectUris=new[]
                    { 
                        "http://127.0.0.1:8887/signout-callback-oidc",
                        "http://127.0.0.1:3000/signout-callback-oidc",
                        "http://localhost:3000/signout-callback-oidc",
                        "https://psl-app-vm3/BusinessEbookkeeping/signout-callback-oidc"
                    },
                    AllowedScopes =   {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ebookkeeping-api"
                        },
                    AllowOfflineAccess = true,
                    RequireClientSecret= false,
                    AllowAccessTokensViaBrowser=true

                },

                // native clients
                new Client
                {
                    ClientId = "native.hybrid",
                    ClientName = "Native Client (Hybrid with PKCE)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = true,
                    AllowedScopes = { "openid", "profile", "email", "api" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
                new Client
                {
                    ClientId = "server.hybrid",
                    ClientName = "Server-based Client (Hybrid)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = { "openid", "profile", "email", "api" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
                new Client
                {
                    ClientId = "native.code",
                    ClientName = "Native Client (Code with PKCE)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowedScopes = { "openid", "profile", "email", "api" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
                new Client
                {
                    ClientId = "server.code",
                    ClientName = "Service Client (Code)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "openid", "profile", "email", "api" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },

                // server to server
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api" },
                },

                // SPA per new security guidance
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA (Code + PKCE)",

                    RequireClientSecret = false,

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "openid", "profile", "email", "api" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },

                // implicit (e.g. SPA or OIDC authentication)
                new Client
                {
                    ClientId = "implicit",
                    ClientName = "Implicit Client",
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },
                    FrontChannelLogoutUri = "http://localhost:5000/signout-idsrv", // for testing identityserver on localhost

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api" },
                },

                // implicit using reference tokens (e.g. SPA or OIDC authentication)
                new Client
                {
                    ClientId = "implicit.reference",
                    ClientName = "Implicit Client using reference tokens",
                    AllowAccessTokensViaBrowser = true,

                    AccessTokenType = AccessTokenType.Reference,

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api" },
                },

                // implicit using reference tokens (e.g. SPA or OIDC authentication)
                new Client
                {
                    ClientId = "implicit.shortlived",
                    ClientName = "Implicit Client using short-lived tokens",
                    AllowAccessTokensViaBrowser = true,

                    AccessTokenLifetime = 70,

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email", "api" },
                },
                // device flow
                new Client
                {
                    ClientId = "device",
                    ClientName = "Device Flow Client",

                    //AllowedGrantTypes = GrantTypes.DeviceFlow,
                    RequireClientSecret = false,

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "api" }
                },

            };
        }
    }
}
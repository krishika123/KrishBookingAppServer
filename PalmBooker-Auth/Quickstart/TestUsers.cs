// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{SubjectId = "2EBC9CA6-A0BB-4383-BF25-5EEA240C3B32", Username = "system", Password = "System123!",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "System Admin"),
                    new Claim(JwtClaimTypes.GivenName, "System"),
                    new Claim(JwtClaimTypes.FamilyName, "Admin"),
                    new Claim(JwtClaimTypes.Email, "paul.kodjo@persol.net"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://system.com"),
                    new Claim(JwtClaimTypes.Picture, "https://instagram.facc5-1.fna.fbcdn.net/v/t51.2885-15/e35/p1080x1080/124195321_365869317953375_2212079457914856446_n.jpg?_nc_ht=instagram.facc5-1.fna.fbcdn.net&_nc_cat=111&_nc_ohc=-MZTPxo6m30AX9ZgMH8&tp=19&oh=37f764c6157bd2a7ec603ba8437a8a8c&oe=5FD56E10"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'Akleme', 'locality': 'Odumase', 'postal_code': 155, 'country': 'Ghana' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("role", "admin"),
                    new Claim("department", "admin")
                }
            }
        };
    }
}
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityExpress.Identity;
using IdentityExpress.Manager.Api;
using IdentityServer4;
using IdentityServer4.Configuration;
using EBookkeepingAuth.Data;
using EBookkeepingAuth.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.EntityFramework.Mappers;
using System;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using IdentityServer4.Services;
using EBookkeepingAuth.Services;
using RazorHtmlEmails.Common;
using Microsoft.AspNetCore.Identity.UI.Services;
using RazorHtmlEmails.RazorClassLib.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Diagnostics;
using CoreFlogger;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using EBookkeepingAuth.Data.CompanyContext;

namespace EBookkeepingAuth
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //IdentityModelEventSource.ShowPII = true;
            services.AddControllersWithViews();
            //services.AddMvc();
            services.AddRazorPages();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            //services.Configure<IISOptions>(iis =>
            //{
            //    iis.AuthenticationDisplayName = "Windows";
            //    iis.AutomaticAuthentication = false;
            //});

            // configures IIS in-proc settings
            //services.Configure<IISServerOptions>(iis =>
            //{
            //    iis.AuthenticationDisplayName = "Windows";
            //    iis.AutomaticAuthentication = false;
            //});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IDS")));
            services.AddDbContext<EbookkeepingApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IDS")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {

                    //options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromHours(1);
                    //    options => {
                    //    options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromHours(1);
                }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                //.AddDefaultUI()
                .AddDefaultTokenProviders();




            var connectionString = Configuration.GetConnectionString("IDS");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var builder = services.AddIdentityServer(options =>
                {
                    options.IssuerUri = Configuration["IDPSettingsConfigurations:IssuerURL"];
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    options.UserInteraction = new UserInteractionOptions
                    {
                        LogoutUrl = "/Account/Logout",
                        LoginUrl = "/Account/Login",
                        LoginReturnUrlParameter = "returnUrl"
                        
                    };
                })
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources, CORS)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    // options.TokenCleanupInterval = 15; // interval in seconds. 15 seconds useful for debugging
                });

            //Certificate
            var cert = new X509Certificate2(Path.Combine(Environment.ContentRootPath, "idsrv3test.pfx"), "idsrv3test");


            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                builder.AddSigningCredential(cert);
            }

            services.AddAuthentication(IdentityConstants.ApplicationScheme);
                //.AddGoogle(options =>
               // {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                  //  options.ClientId = "copy client ID from Google here";
                  //  options.ClientSecret = "copy client secret from Google here";
              //  });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
            services.AddTransient<IProfileService, ProfileService>();
            services.AddSingleton<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    Configuration["EmailSender:UserName"],
                    Configuration["EmailSender:Password"],
                    Configuration["EmailSender:ApiKey"],
                    Configuration["EmailSender:UserEmail"]
                )
            );

            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(o => 
            {
                o.ViewLocationFormats.Add("/Views/{0}" + Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine.ViewExtension);
                //o.FileProviders.Add(new Microsoft.Extensions.FileProviders.PhysicalFileProvider(AppContext.BaseDirectory));
            }
            );

            // In Startup.cs in the ConfigureServices method
            services.AddScoped<IRegisterAccountService, RegisterAccountService>();
            services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddScoped<IEbookkeepingUserManager, EbookkeepingUserManager>();


        }

        public void Configure(IApplicationBuilder app)
        {
            //MigrateInMemoryDataToSqlServer(app);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseRouting();
           
            app.UseAuthorization();
            CustomExceptionHandler(app);

            app.UseEndpoints(endpoints =>

            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

        }

        public void MigrateInMemoryDataToSqlServer(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var testUser in TestUsers.Users)
                    {
                        var identityUser = new ApplicationUser(testUser.Username)
                        {
                            Id = testUser.SubjectId,
                            Email = testUser.Claims.FirstOrDefault(p => p.Type == "email").Value,
                            //Profile = testUser.Claims.FirstOrDefault(p => p.Type == "picture").Value,
                            //BusinessType="LLC",
                            DateCreated = DateTime.Now,
                             //BusinessAddress ="Airport Residential Area",
                             //BusinessName= "Persol Systems And Electronics",
                             //BusinessTIN ="C0001425896",
                             //GhanaCardNumber="",
                             PhoneNumber="0205429916",
                            // MobileNumber="0249950137",
                             //OtherMobileNumber=""
                        };
                        userManager.CreateAsync(identityUser, testUser.Password).Wait();
                        userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList()).Wait();
                    }
                }
            }
        }

        private static void CustomExceptionHandler(IApplicationBuilder app)
        {
            app.UseExceptionHandler(eApp =>
            {
                eApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorCtx = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorCtx != null)
                    {
                        var ex = errorCtx.Error;
                        WebHelper.LogWebError("E-Bookkeeping Auth", "E-Bookkeeping Auth", ex, context);

                        var errorId = Activity.Current?.Id ?? context.TraceIdentifier;
                        var jsonResponse = JsonConvert.SerializeObject(new CustomErrorResponse
                        {
                            ErrorId = errorId,
                            Message = "Some Error Happened."
                        });
                        await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);
                    }
                });
            });
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBookkeepingAuth.Models;
using IdentityServer4.Events;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EBookkeepingAuth.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        [HttpPost("UserLogin",Name = "UserLogin")]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (ModelState.IsValid)
            {    
                string userName = model.Username;
                var userByEmail = await _userManager.FindByEmailAsync(model.Username);
                if (userByEmail != null)
                {
                    userName = userByEmail.UserName;
                }

                var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(userName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(userName, user.Id, user.UserName, clientId: context?.ClientId));

                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.

                            return Ok(new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null

                        return Ok(new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Ok(new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Ok(new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        return Ok(new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }
                }
                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                throw new Exception("Invalid Credentials");

            }

            throw new Exception("Invalid Credentials");

        }
    }
}

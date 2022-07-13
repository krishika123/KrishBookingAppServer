using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using EBookkeepingAuth.Models;
using Microsoft.AspNetCore.Identity;
using EBookkeepingAuth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using IdentityServer4.Test;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity.UI.Services;
using RazorHtmlEmails.Common;
using EBookkeepingAuth.Data.CompanyContext;

namespace EBookkeepingAuth.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IRegisterAccountService _registerAccountService;
        private readonly IEbookkeepingUserManager _ebUserManager;



        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IRegisterAccountService registerAccountService,
            IEbookkeepingUserManager ebUserManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _registerAccountService = registerAccountService;
            _ebUserManager = ebUserManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Other Names")]
            public string OtherNames { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Ghana Card #")]
            public string GhanaCardNumber { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Business Name")]
            public string BusinessName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Business Email")]
            public string BusinessEmail { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Business Phone Number")]
            public string BusinessPhoneNumber { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Business Registration #")]
            public string BusinessRegistrationNumber { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Business Address")]
            public string BusinessAddress { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Business TIN")]
            public string BusinessTIN { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var identityUser = new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = Input.Email,
                    Password = Input.Password,
                    Claims ={
                    new Claim(JwtClaimTypes.Name, Input.FirstName + " " + Input.LastName),
                    new Claim(JwtClaimTypes.GivenName, Input.GhanaCardNumber),
                    new Claim(JwtClaimTypes.FamilyName, Input.FirstName),
                    new Claim(JwtClaimTypes.Email, Input.Email),
                    //new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean),
                    //new Claim(JwtClaimTypes.Picture, "https://instagram.facc5-1.fna.fbcdn.net/v/t51.2885-15/e35/p1080x1080/124195321_365869317953375_2212079457914856446_n.jpg?_nc_ht=instagram.facc5-1.fna.fbcdn.net&_nc_cat=111&_nc_ohc=-MZTPxo6m30AX9ZgMH8&tp=19&oh=37f764c6157bd2a7ec603ba8437a8a8c&oe=5FD56E10"),
                    //new Claim(JwtClaimTypes.Address, Input.BusinessAddress, IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                    //new Claim("role", "user"),
                }
                };
                var user = new ApplicationUser(identityUser.Username)
                {
                    Id = identityUser.SubjectId,
                    UserName = Input.GhanaCardNumber,
                    Email = Input.Email,
                    //GhanaCardNumber = Input.GhanaCardNumber,
                    //BusinessRegistrationNumber=Input.BusinessRegistrationNumber,
                    PhoneNumber = Input.PhoneNumber,
                    //FirstName=Input.FirstName,
                    //LastName=Input.LastName,
                    //MiddleName=Input.OtherNames,
                    //BusinessAddress =Input.BusinessAddress,
                    //BusinessName = Input.BusinessName,
                    //BusinessTIN = Input.BusinessTIN,
                    //BusinessType = Input.BusinessType,
                    //Profile = identityUser.Claims.FirstOrDefault(p => p.Type == "picture").Value,
                    DateCreated = DateTime.Now

                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    _userManager.AddClaimsAsync(user, identityUser.Claims.ToList()).Wait();
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page("/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    var htmlBody = await _registerAccountService.RegisterAccountHtmlBody(callbackUrl, Url.Page("./Index"));

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm Account", htmlBody);

                    //Create Company Account Here And Associate User
                    Companies company = new Companies() 
                    { 
                         Status="A",
                         TransactionCurrency= Guid.Parse("88319A3C-1933-4846-9270-1667CBE2F7A1"),
                         Email =Input.BusinessEmail,
                         Mobile=Input.BusinessPhoneNumber,
                         CompanyTin=Input.BusinessTIN,
                          BusinessRegistrationNumber=Input.BusinessRegistrationNumber,
                          Phone=Input.BusinessPhoneNumber,
                          CompanyName=Input.BusinessName,
                          GhPostGps=null,
                          UserId=Guid.Parse(user.Id)
                           
                    };
                    var sCompany = await _ebUserManager.AddCompanyAsync(company);
                    if (sCompany != null)
                    {
                        CompanyAssociation companyAssociation = new CompanyAssociation()
                        {
                            AssociatedUserId= user.Id,
                            CompanyId = sCompany.Id,
                            UserId = user.Id,
                            IsAdmin = true,
                            Status = "A"
                        };
                        await _ebUserManager.AddCompanyAssociationAsync(companyAssociation);

                    }

                    _userManager.Options.SignIn.RequireConfirmedAccount = false;
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

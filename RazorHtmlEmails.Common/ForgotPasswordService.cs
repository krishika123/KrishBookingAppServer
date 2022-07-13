using RazorHtmlEmails.RazorClassLib.Services;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ConfirmAccount;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ForgotPassword;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RazorHtmlEmails.Common
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public ForgotPasswordService(IRazorViewToStringRenderer razorViewToStringRenderer
            )
        {
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }
        public async Task ForgotPassword(string email, string callbackUrl, string baseUrl)
        {
            // TODO: Validation + actually add the User to a DB + whatever else
            // TODO: Base URL off of ASP.NET Core Identity's logic or some other mechanism, rather than hardcoding to creating a random guid
            var confirmAccountModel = new ConfirmAccountEmailViewModel($"{baseUrl}/{Guid.NewGuid()}");

            string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);
            var toAddresses = new List<string> { email };

            //await _emailSender.SendEmailAsync(toAddresses, "Confirm your Account", body);
            //SendEmail(toAddresses, "donotreply@contoso.com", "Confirm your Account", body);
        }
        public async Task<string> ForgotPasswordHtmlBody(string callbackUrl,string baseUrl)
        {
            // TODO: Validation + actually add the User to a DB + whatever else
            // TODO: Base URL off of ASP.NET Core Identity's logic or some other mechanism, rather than hardcoding to creating a random guid
            var forgotPasswordModel = new ForgotPasswordEmailViewModel(callbackUrl);
            string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ForgotPassword/ForgotPasswordEmail.cshtml", forgotPasswordModel);
            return body;

        }
    }
    public interface IForgotPasswordService
    {
        Task ForgotPassword(string email, string callbackUrl, string baseUrl);
        Task<string> ForgotPasswordHtmlBody(string callbackUrl,string baseUrl);

    }
}


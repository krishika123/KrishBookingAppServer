﻿using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using RazorHtmlEmails.RazorClassLib.Services;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ConfirmAccount;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorHtmlEmails.Common
{
    public class RegisterAccountService : IRegisterAccountService
    {
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public RegisterAccountService(IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }

        public async Task Register(string email, string baseUrl)
        {
            // TODO: Validation + actually add the User to a DB + whatever else
            // TODO: Base URL off of ASP.NET Core Identity's logic or some other mechanism, rather than hardcoding to creating a random guid
            var confirmAccountModel = new ConfirmAccountEmailViewModel($"{baseUrl}/{Guid.NewGuid()}");

            string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);

            var toAddresses = new List<string> { email };

            SendEmail(toAddresses, "donotreply@contoso.com", "Confirm your Account", body);
        }

        public async Task<string> RegisterAccountHtmlBody(string callbackUrl, string baseUrl)
        {
            // TODO: Validation + actually add the User to a DB + whatever else
            // TODO: Base URL off of ASP.NET Core Identity's logic or some other mechanism, rather than hardcoding to creating a random guid
            var confirmAccountModel = new ConfirmAccountEmailViewModel(callbackUrl);
            string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml", confirmAccountModel);
            return body;

        }

        // TODO: In reality, you probably want to make an EmailService that houses this code, but #Demoware
        private void SendEmail(List<string> toAddresses, string fromAddress, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromAddress));
            foreach (var to in toAddresses)
            {
                message.To.Add(new MailboxAddress(to));
            }
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("127.0.0.1", 25, false);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }

    public interface IRegisterAccountService
    {
        Task Register(string email, string baseUrl);
        Task<string> RegisterAccountHtmlBody(string callbackUrl, string baseUrl);
    }
}

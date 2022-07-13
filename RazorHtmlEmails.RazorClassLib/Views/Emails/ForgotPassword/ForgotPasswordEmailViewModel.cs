using System;
using System.Collections.Generic;
using System.Text;

namespace RazorHtmlEmails.RazorClassLib.Views.Emails.ForgotPassword
{
    public class ForgotPasswordEmailViewModel
    {
        public ForgotPasswordEmailViewModel(string resetEmailUrl)
        {
            ResetEmailUrl = resetEmailUrl;
        }

        public string ResetEmailUrl { get; set; }
    }
}
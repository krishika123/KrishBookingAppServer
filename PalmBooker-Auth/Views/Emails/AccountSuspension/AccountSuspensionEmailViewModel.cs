using System;
using System.Collections.Generic;
using System.Text;

namespace RazorClassLib.Pages.Emails.AccountSuspension
{
    public class AccountSuspensionEmailViewModel
    {
        public AccountSuspensionEmailViewModel(string accountRecoveryUrl)
        {
            AccountRecoveryUrl = accountRecoveryUrl;
        }

        public string AccountRecoveryUrl { get; set; }
    }
}

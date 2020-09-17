using System;
using System.Collections.Generic;
using System.Text;
using UniSA.Services.EmailServices.MailDomain;

namespace UniSA.Services.EmailServices.Interfaces
{
    public interface IEmailService
    {
        string SmtpClient { get; set; }
        bool SendEmail(MailMessage Message);
    }
}

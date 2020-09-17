using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UniSA.Services.EmailServices.Interfaces;
using UniSA.Services.EmailServices.MailDomain;

namespace UniSA.Services.EmailServices
{
    public class UniSAEmailService : IEmailService
    {
        public enum EmailType { Text, Html}

        private SmtpClient _smtpServer;
        public string SmtpClientString { get; set; }
        public UniSAEmailService(string smtpHostServer, string smtpServerUsername, string smtpServerPassword)
        {
            SmtpClientString = smtpHostServer;
            _smtpServer = new SmtpClient(SmtpClientString);
            EmailContentType = EmailType.Text;
            SmtpServerUsername = smtpServerUsername;
            SmtpServerPassword = smtpServerPassword;
        }
        public string SmtpServerUsername { get; set; }
        public string SmtpServerPassword { get; set; }
        public EmailType EmailContentType { get; set; }
        public string SmtpClient { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool SendEmail(UniSA.Services.EmailServices.MailDomain.MailMessage message)
        {
            try
            {
                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new MailAddress(message.EmailFrom);
                foreach (var to in message.EmailTo)
                {
                    mailMessage.To.Add(new MailAddress(to));
                }
                if (!string.IsNullOrEmpty(message.AttachmentFilePath))
                    mailMessage.Attachments.Add(new Attachment(message.AttachmentFilePath));

                mailMessage.Subject = message.Subject;
                mailMessage.IsBodyHtml = EmailContentType == EmailType.Html;
                mailMessage.Body = message.EmailMessage;
                _smtpServer.Credentials = new NetworkCredential(SmtpServerUsername, SmtpServerPassword);

                _smtpServer.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}

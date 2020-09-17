using System;
using System.Collections.Generic;
using System.Text;

namespace UniSA.Services.EmailServices.MailDomain
{
    public class MailMessage
    {
        public string EmailMessage { get; set; }
        public IList<string> EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string AttachmentFilePath { get; set; }
        public string Subject { get; set; }
    }
}

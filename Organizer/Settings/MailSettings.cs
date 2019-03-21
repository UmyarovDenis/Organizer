using System;
using System.Net.Mail;

namespace Organizer.Settings
{
    [Serializable]
    public class MailSettings
    {
        public string ServiceHost { get; set; }
        public string ServicePassword { get; set; }
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public bool EnableSSL { get; set; }
        public int Timeout { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
    }
}

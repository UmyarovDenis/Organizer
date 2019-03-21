using Organizer.Settings;
using Organizer.Utils;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Organizer.Services.Net
{
    internal class MailService : IMailService
    {
        public bool IsValidMail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        public Task SendMail(string email, string subject, string text, string[] attachments)
        {
            return Task.Run(() => 
            {
                MailSettings mailSettings = Formatter.GetSettings<MailSettings>(@"Settings\MailSettings.mconf");

                MailAddress from = new MailAddress(mailSettings.ServiceHost);
                MailAddress to = new MailAddress(email);
                MailMessage mailMessage = new MailMessage(from, to);

                mailMessage.Subject = subject;
                mailMessage.Body = text;

                if(attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        mailMessage.Attachments.Add(new Attachment(attachment));
                    }
                }

                SmtpClient smtpClient = new SmtpClient();

                smtpClient.Host = mailSettings.SMTPHost;
                smtpClient.Port = mailSettings.SMTPPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = mailSettings.DeliveryMethod;
                smtpClient.Credentials = new NetworkCredential(from.Address, mailSettings.ServicePassword);
                smtpClient.EnableSsl = mailSettings.EnableSSL;
                smtpClient.Timeout = mailSettings.Timeout;

                smtpClient.Send(mailMessage);
            });
        }
    }
}

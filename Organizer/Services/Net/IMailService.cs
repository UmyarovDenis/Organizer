using System.Threading.Tasks;

namespace Organizer.Services.Net
{
    internal interface IMailService
    {
        bool IsValidMail(string email);
        Task SendMail(string email, string subject, string text, string[] attachments);
    }
}

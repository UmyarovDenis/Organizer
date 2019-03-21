using System.Collections.Generic;

namespace Organizer.Models
{
    internal sealed class MailMessageDto
    {
        public MailMessageDto()
        {
            Attachments = new List<string>();
        }
        public string Subject { get; set; }
        public string Text { get; set; }
        public List<string> Attachments { get; set; }
    }
}

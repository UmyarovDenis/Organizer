using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using System.Data.Entity;

namespace DataTransferService.DataAccessLayer.Repositories
{
    internal sealed class MailRepository : EFRepository<Mail>, IMailRepository
    {
        public MailRepository()
        {
        }
    }
}

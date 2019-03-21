using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using System.Data.Entity;
using System.Linq;

namespace DataTransferService.DataAccessLayer.Repositories
{
    internal sealed class TaskRepository : EFRepository<Task>, ITaskRepository
    {
        public TaskRepository()
        {
            _navProps.Add(t => t.Organization);
            _navProps.Add(t => t.Organization.City);
            _navProps.Add(t => t.Organization.City.Region);
            _navProps.Add(t => t.Organization.Contacts);
            _navProps.Add(t => t.Mails);
        }
    }
}

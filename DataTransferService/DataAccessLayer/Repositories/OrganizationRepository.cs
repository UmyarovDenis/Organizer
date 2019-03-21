using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using System.Data.Entity;
using System.Linq;

namespace DataTransferService.DataAccessLayer.Repositories
{
    internal class OrganizationRepository : EFRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository()
        {
            _navProps.Add(o => o.City);
            _navProps.Add(o => o.City.Region);
            _navProps.Add(o => o.Contacts);
            _navProps.Add(o => o.Tasks.Select(t => t.Mails));
        }
    }
}

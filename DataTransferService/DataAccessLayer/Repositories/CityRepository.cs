using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using System.Data.Entity;
using System.Linq;

namespace DataTransferService.DataAccessLayer.Repositories
{
    internal sealed class CityRepository : EFRepository<City>, ICityRepository
    {
        public CityRepository()
        {
            _navProps.Add(c => c.Region);
            _navProps.Add(c => c.Organizations.Select(o => o.Contacts));
            _navProps.Add(c => c.Organizations.Select(o => o.Tasks.Select(t => t.Mails)));
        }
    }
}

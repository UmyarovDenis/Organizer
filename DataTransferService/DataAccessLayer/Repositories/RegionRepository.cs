using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using System.Linq;
using System.Data.Entity;

namespace DataTransferService.DataAccessLayer.Repositories
{
    internal class RegionRepository : EFRepository<Region>, IRegionRepository
    {
        public RegionRepository()
        {
            _navProps.Add(r => r.Cities.Select(c => c.Organizations.Select(o => o.Tasks.Select(t => t.Mails))));
            _navProps.Add(r => r.Cities.Select(c => c.Organizations.Select(o => o.Contacts)));
        }
    }
}

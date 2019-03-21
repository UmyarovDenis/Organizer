using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataTransferService.DataAccessLayer.Repositories
{
    internal sealed class ContactRepository : EFRepository<Contact>, IContactRepository
    {
        public ContactRepository()
        {
            _navProps.Add(c => c.Organization);
            _navProps.Add(c => c.Organization.City);
            _navProps.Add(c => c.Organization.City.Region);
            _navProps.Add(c => c.Organization.City.Region);
            _navProps.Add(c => c.Organization.Tasks.Select(t => t.Mails));
        }
    }
}

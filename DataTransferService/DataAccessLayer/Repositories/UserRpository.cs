using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;

namespace DataTransferService.DataAccessLayer.Repositories
{
    internal sealed class UserRpository : EFRepository<User>, IUserRepository
    {
    }
}

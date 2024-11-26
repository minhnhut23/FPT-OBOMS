using BusinessObject.Models;

namespace DataAccess.IRepository;

public interface IOwnerRepository
{
    Task<List<Owner>> GetAllOwners();
    Task<Owner?> GetOwnerByUsername(string username);
    Task AddOwner(Owner owners);
}

using BusinessObject.Models;

namespace DataAccess.IRepository;

public interface IOwnerRepository
{
    Task<List<Owners>> GetAllOwners();
    Task<Owners?> GetOwnerByUsername(string username);
    Task AddOwner(Owners owners);
}

using BusinessObject.Models;
using DataAccess.DAO;

namespace DataAccess.IRepository.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly OwnerDAO _ownerDAO;

    public OwnerRepository(OwnerDAO ownerDAO)
    {
        _ownerDAO = ownerDAO;
    }

    public Task AddOwner(Owner owners)
    {
        return Task.FromResult(_ownerDAO.AddOwner(owners));
    }

    public async Task<List<Owner>> GetAllOwners()
    {
        var owners = await _ownerDAO.GetAllOwners();
        return owners;
    }

    public async Task<Owner?> GetOwnerByUsername(string username)
    {
        var owner = await _ownerDAO.GetOwnerByUsername(username);
        return owner;
    }
}

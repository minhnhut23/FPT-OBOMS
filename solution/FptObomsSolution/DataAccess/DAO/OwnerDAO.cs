using BusinessObject.Models;
using Supabase;

namespace DataAccess.DAO;

public class OwnerDAO
{
    private readonly Supabase.Client _client;

    public OwnerDAO(Client client)
    {
        _client = client;
    }

    public async Task<List<Owner>> GetAllOwners()
    {
        var result = await _client.From<Owner>().Get();
        return result.Models;
    }

    public async Task<Owner?> GetOwnerByUsername(string username)
    {
        var result = await _client
            .From<Owner>()
            .Filter("user_name", Supabase.Postgrest.Constants.Operator.Equals, username)
            .Get();

        return result.Models.FirstOrDefault();
    }

    public async Task AddOwner(Owner owner)
    {
        await _client.From<Owner>().Insert(owner);
    }
}

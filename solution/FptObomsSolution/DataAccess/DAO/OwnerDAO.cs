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

    public async Task<List<Owners>> GetAllOwners()
    {
        var result = await _client.From<Owners>().Get();
        return result.Models;
    }

    public async Task<Owners?> GetOwnerByUsername(string username)
    {
        var result = await _client
            .From<Owners>()
            .Filter("user_name", Supabase.Postgrest.Constants.Operator.Equals, username)
            .Get();

        return result.Models.FirstOrDefault();
    }

    public async Task AddOwner(Owners owner)
    {
        await _client.From<Owners>().Insert(owner);
    }
}

using Supabase;

namespace DataAccess.DAO;

public class ManageShopDAO
{
    private readonly Supabase.Client _client;

    public ManageShopDAO(Client client)
    {
        _client = client;
    }
}

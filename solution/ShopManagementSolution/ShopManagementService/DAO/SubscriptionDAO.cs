using BusinessObject.DTOs.ShopDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Supabase;

namespace ShopManagementService.DAO;

public class SubscriptionDAO
{
    private readonly Client _client;

    public SubscriptionDAO(Client client)
    {
        _client = client;
    }

    public async Task<List<Subscriptions>> GetAll()
    {
        try
        {
            var response = await _client.From<Subscriptions>().Get();


            return response.Models;
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }


}

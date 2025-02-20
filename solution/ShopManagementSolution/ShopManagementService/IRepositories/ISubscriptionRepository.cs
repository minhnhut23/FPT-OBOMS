using BusinessObject.Models;

namespace ShopManagementService.IRepositories;

public interface ISubscriptionRepository
{
    public Task<List<Subscriptions>> GetAllSubscriptions();

}

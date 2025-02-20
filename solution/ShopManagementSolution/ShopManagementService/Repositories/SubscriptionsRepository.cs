using BusinessObject.Models;
using ShopManagementService.DAO;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories;

public class SubscriptionsRepository : ISubscriptionRepository
{
    private readonly SubscriptionDAO _dao;

    public SubscriptionsRepository(SubscriptionDAO dao)
    {
        _dao = dao;
    }

    public Task<List<Subscriptions>> GetAllSubscriptions() => _dao.GetAll();
}

using BusinessObject.DTOs.SubscriptionDTO;
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

    public Task CreateSubscription(CreateSubscriptionRequestDTO request) => _dao.Create(request);

    public Task<List<SubscriptionResponseDTO>> GetAllSubscriptions() => _dao.GetAll();
}

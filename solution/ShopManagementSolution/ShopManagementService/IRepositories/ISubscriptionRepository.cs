using BusinessObject.DTOs.SubscriptionDTO;
using BusinessObject.Models;

namespace ShopManagementService.IRepositories;

public interface ISubscriptionRepository
{
    public Task<List<SubscriptionResponseDTO>> GetAllSubscriptions();
    public Task CreateSubscription(CreateSubscriptionRequestDTO request);
}

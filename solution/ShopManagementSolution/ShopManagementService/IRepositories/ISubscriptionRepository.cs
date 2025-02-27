using BusinessObject.DTOs.SubscriptionDTO;
using BusinessObject.Models;

namespace ShopManagementService.IRepositories;

public interface ISubscriptionRepository
{
    public Task<List<SubscriptionResponseDTO>> GetAllSubscriptions();
    public Task CreateSubscription(CreateSubscriptionRequestDTO request);
    public Task<SubscriptionResponseDTO> GetById(Guid requestId);
    public Task<SubscriptionResponseDTO> Update(Guid requestId, UpdateSubscriptionRequestDTO request);
}

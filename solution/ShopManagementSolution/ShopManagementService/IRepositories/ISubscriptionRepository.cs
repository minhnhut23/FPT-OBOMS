using BusinessObject.DTOs.SubscriptionDTO;
using BusinessObject.Models;

namespace ShopManagementService.IRepositories;

public interface ISubscriptionRepository
{
    Task<List<SubscriptionResponseDTO>> GetAllSubscriptions();
    Task CreateSubscription(CreateSubscriptionRequestDTO request);
    Task<SubscriptionResponseDTO> GetById(Guid requestId);
    Task<SubscriptionResponseDTO> Update(Guid requestId, UpdateSubscriptionRequestDTO request);
}

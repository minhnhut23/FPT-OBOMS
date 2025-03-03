using BusinessObject.DTOs.ReviewDTO;
using BusinessObject.DTOs.TableDTO;

namespace ShopManagementService.IRepositories;

public interface IReviewRepository
{
    public Task<(List<ReviewResponseDTO> Reviews, TablePaginationDTO PaginationMetadata)> GetAll(GetReviewDTO request);
    public Task<ReviewResponseDTO> Create(CreateReviewRequestDTO request, string token);
}

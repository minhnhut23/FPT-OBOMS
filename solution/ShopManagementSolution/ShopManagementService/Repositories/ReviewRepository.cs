using BusinessObject.DTOs.ReviewDTO;
using BusinessObject.DTOs.TableDTO;
using ShopManagementService.DAO;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewDAO _dao;

    public ReviewRepository(ReviewDAO dao)
    {
        _dao = dao;
    }

    public Task<(List<ReviewResponseDTO> Reviews, TablePaginationDTO PaginationMetadata)> GetAll(GetReviewDTO request) => _dao.GetAll(request);
}

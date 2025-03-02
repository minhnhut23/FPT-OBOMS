using BusinessObject.DTOs.ReviewDTO;
using BusinessObject.DTOs.ShopDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Supabase;
using static Supabase.Postgrest.Constants;

namespace ShopManagementService.DAO;

public class ReviewDAO
{
    private readonly Client _client;

    public ReviewDAO(Client client)
    {
        _client = client;
    }

    public async Task<(List<ReviewResponseDTO> Reviews, TablePaginationDTO PaginationMetadata)> GetAll(GetReviewDTO request)
    {
        try
        {
            var query = _client.From<Review>()
                .Select("*");

            var totalRecordsResponse = await _client.From<Review>().Select("id").Get();
            var totalRecords = totalRecordsResponse.Models?.Count ?? 0;
            var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedQuery = query.Range(skip, skip + request.PageSize - 1);

            var reviewResponse = await paginatedQuery.Get();
            var customerIds = reviewResponse.Models.Select(r => r.CustomerId).Distinct().ToList();

            var profilesResponse = await _client
            .From<Profile>()
            .Select("id, full_name")
            .Filter("id", Operator.In, customerIds)
            .Get();

            var profilesDict = profilesResponse.Models?.ToDictionary(p => p.Id, p => p.FullName) ?? new Dictionary<Guid, string>();

            var reviews = reviewResponse.Models.Select(r => new ReviewResponseDTO
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = profilesDict.TryGetValue(r.CustomerId, out var name) ? name : "Unknown",
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                Rating = r.Rating,
            }).ToList();

            if (totalRecords == 0 || request.PageNumber > totalPages)
            {
                return (
                    new List<ReviewResponseDTO>(),
                    new TablePaginationDTO
                    {
                        TotalResults = totalRecords,
                        TotalPages = totalPages,
                        CurrentPage = request.PageNumber,
                        PageSize = request.PageSize
                    }
                );
            }

            var paginationMetadata = new TablePaginationDTO
            {
                TotalResults = totalRecords,
                TotalPages = totalPages,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize
            };

            return (reviews, paginationMetadata);
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

}

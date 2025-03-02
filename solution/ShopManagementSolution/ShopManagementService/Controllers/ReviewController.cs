using BusinessObject.DTOs.ReviewDTO;
using Microsoft.AspNetCore.Mvc;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Controllers;

public class ReviewController : Controller
{
    private readonly IReviewRepository _repo;

    public ReviewController(IReviewRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReviews([FromQuery] GetReviewDTO request)
    {
        try
        {
            var reviews = await _repo.GetAll(request);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

using BusinessObject.DTOs.ReviewDTO;
using BusinessObject.Enums;
using Microsoft.AspNetCore.Mvc;
using ShopManagementService.IRepositories;
using ShopManagementService.Utils.Security;

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
            return BadRequest(new { msg = ex.Message });
        }
    }

    [HttpPost]
    [AuthorizeRole(UserRole.Customer)]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequestDTO request)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");


            var createdShop = await _repo.Create(request, token);
            return Ok(createdShop);
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });

        }

    }
}

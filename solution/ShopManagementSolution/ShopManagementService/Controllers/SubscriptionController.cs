using Microsoft.AspNetCore.Mvc;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : Controller
{
    private readonly ISubscriptionRepository _repo;

    public SubscriptionController(ISubscriptionRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try        
        {
            return Ok(await _repo.GetAllSubscriptions());
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }
    
}

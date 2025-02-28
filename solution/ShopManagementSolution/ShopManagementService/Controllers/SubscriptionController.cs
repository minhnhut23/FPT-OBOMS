using BusinessObject.DTOs.SubscriptionDTO;
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

    [HttpPost]
    public async Task<IActionResult> Create(CreateSubscriptionRequestDTO request)
    {
        try
        {
            await _repo.CreateSubscription(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            return Ok(await _repo.GetById(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateSubscriptionRequestDTO request)
    {
        try
        {
            return Ok(await _repo.Update(id, request));
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }
}

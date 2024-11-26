using BusinessObject.Models;
using DataAccess.IRepository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace OwnerServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    private readonly OwnerRepository _ownerRepository;

    public OwnerController(OwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Owner>>> GetAllActiveOwners()
    {
        var owners = await _ownerRepository.GetAllOwners();
        return Ok(owners);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<Owner>> GetOwnerByUsername(string username)
    {
        var owner = await _ownerRepository.GetOwnerByUsername(username);
        if (owner == null)
            return NotFound();

        return Ok(owner);
    }

    [HttpPost]
    public async Task<IActionResult> AddOwner([FromBody] Owner owner)
    {
        await _ownerRepository.AddOwner(owner);
        return Created();
    }
}

using AuthService.DAO;
using AuthService.IRepositories;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController : Controller
{
    private readonly IUserRepository _repo;

    public UserController(IUserRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("getCurrentProfile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _repo.GetCurrentUser(token);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }

    }

    [HttpGet("getUserById")]
    [Authorize]
    public async Task<IActionResult> GetUserById([FromQuery] Guid id)
    {
        try
        {
            var reponse = await _repo.GetUserById(id);
            return Ok(reponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

    [HttpPut("updateProfile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDTO request)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var reponse = await _repo.UpdateProfile(request, token);
            return Ok(reponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }
}

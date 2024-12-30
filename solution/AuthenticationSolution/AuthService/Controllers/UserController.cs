using AuthService.DAO;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController : Controller
{
    private readonly UserDAO _dao;

    public UserController(UserDAO dao)
    {
        _dao = dao;
    }

    [HttpGet("getCurrentProfile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _dao.GetCurrentUser(token);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }

    }

    [HttpGet("createProfile")]
    [Authorize]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequestDTO request)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var reponse = await _dao.CreateUser(request, token);
            return Ok(reponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }

    }

    [HttpGet("getUserById")]
    public async Task<IActionResult> GetUserById([FromQuery] Guid id)
    {
        try
        {
            var reponse = await _dao.GetUserById(id);
            return Ok(reponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }
}

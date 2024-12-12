using AuthService.DAO;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supabase;

namespace TestAuth.Controllers;

[Route("/api/auth")]
public class AuthController : Controller
{
    private readonly AuthDAO _authDAO;

    public AuthController(AuthDAO authDAO)
    {
        _authDAO = authDAO;
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        try
        {
            var loginResponse = await _authDAO.Login(request.Email,request.Password);

            return Ok(loginResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        try
        {
            await _authDAO.Register(request);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}


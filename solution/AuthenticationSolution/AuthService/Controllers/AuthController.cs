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
    public async Task<IActionResult> SignIn([FromBody] LoginRequestDTO request)
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
}


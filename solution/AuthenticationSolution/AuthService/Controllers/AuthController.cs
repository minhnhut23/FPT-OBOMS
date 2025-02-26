using AuthService.IRepositories;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestAuth.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : Controller
{
    private readonly IAuthRepository _repo;

    public AuthController(IAuthRepository repo)
    {
        _repo = repo;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        try
        {
            var loginResponse = await _repo.Login(request.Email, request.Password);

            return Ok(loginResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        try
        {
            await _repo.Register(request);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _repo.Logout();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO requestDTO)
    {
        try
        {
            await _repo.ForgotPassword(requestDTO.Email);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

    [HttpPost("recoverPassword")]
    public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequestDTO requestDTO)
    {
        try
        {
            await _repo.ResetPassword(requestDTO);
            return Ok(new { msg = "Password has been reset successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }
    
    [HttpPost("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO requestDTO)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _repo.ChangePassword(requestDTO, token);
            return Ok(new { msg = "Password has been change successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

}
using AuthService.DAO;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestAuth.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : Controller
{
    private readonly AuthDAO _dao;

    public AuthController(AuthDAO dao)
    {
        _dao = dao;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        try
        {
            var loginResponse = await _dao.Login(request.Email, request.Password);

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
            await _dao.Register(request);

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
            await _dao.Logout();

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
            await _dao.ForgotPassword(requestDTO.Email);

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
            await _dao.ResetPassword(requestDTO);
            return Ok(new { msg = "Password has been reset successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }[HttpPost("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO requestDTO)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _dao.ChangePassword(requestDTO, token);
            return Ok(new { msg = "Password has been change successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { msg = ex.Message });
        }
    }

}
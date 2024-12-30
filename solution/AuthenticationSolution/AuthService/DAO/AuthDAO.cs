using AuthService.Utils;
using BusinessObject.DTO;
using BusinessObject.Models;
using Supabase.Gotrue;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.DAO;

public class AuthDAO
{
    private readonly Supabase.Client _client;

    public AuthDAO(Supabase.Client client)
    {
        _client = client;
    }

    public async Task<LoginResponseDTO> Login(string email, string password)
    {
        try
        {
            var session = await _client.Auth.SignIn(email, password);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(session!.AccessToken);

            var expiresAt = jwtToken.ValidTo;

            return new LoginResponseDTO
            {
                AccessToken = session.AccessToken!,
                RefreshToken = session.RefreshToken!,
                ExpiresAt = expiresAt
            };

        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task Register(RegisterRequestDTO request)
    {
        try
        {
            if (request.Password.Trim() != request.ConfirmPassword.Trim())
            {
                throw new Exception("Password is not valid: password and confirm password are not the same.");
            }

            PasswordValidator validator = new PasswordValidator();

            if (!validator.ValidatePassword(request.Password))
            {
                throw new Exception("Password is not valid: password must contain at least one lowercase, uppercase letter, digit and special character.");
            }

            var session = await _client.Auth.SignUp(request.Email, request.Password);
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task Logout()
    {
        try
        {
            await _client.Auth.SignOut();
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

   /* public async Task ForgotPassword(string email)
    {
        try
        {
            await _client.Auth.ResetPasswordForEmail(email);
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task ResetPassword(string accessToken, string newPassword)
    {
        try
        {
            // Xác thực người dùng bằng Access Token được gửi qua liên kết
            var session = await _client.Auth.VerifyOTP(accessToken, "recovery");

            if (session == null)
            {
                throw new Exception("Invalid or expired token.");
            }

            // Cập nhật mật khẩu mới cho người dùng
            var attrs = new UserAttributes { Password = newPassword };


            var user = await _client.Auth.Update(attrs);

            Console.WriteLine("Password has been reset successfully.");
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }/*
}


using AuthService.Utils;
using BusinessObject.DTO;
using BusinessObject.Models;
using Newtonsoft.Json.Linq;
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
            if (string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                throw new ArgumentException("Password fields cannot be null or empty.");
            }

            if (request.Password != request.ConfirmPassword)
            {
                throw new Exception("Password is not valid: password and confirm password are not the same.");
            }

            if (!PasswordValidator.ValidatePassword(request.Password))
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

    public async Task ForgotPassword(string email)
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

    public async Task ResetPassword(RecoverPasswordRequestDTO request)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(request.AccessToken);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            var email = claims["email"];

            var session = await _client.Auth.VerifyOTP(email, request.AccessToken, Constants.EmailOtpType.Recovery);

            if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.ConfirmPassword)) 
            { 
                throw new ArgumentException("Password fields cannot be null or empty."); 
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                throw new Exception("Password is not valid: password and confirm password are not the same.");
            }

            if (!PasswordValidator.ValidatePassword(request.NewPassword))
            {
                throw new Exception("Password is not valid: password must contain at least one lowercase, uppercase letter, digit and special character.");
            }

            await _client.Auth.Update(new UserAttributes { Password = request.NewPassword });

        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task ChangePassword(ChangePasswordRequestDTO request, string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            if (!claims.ContainsKey("email"))
                throw new Exception("Invalid token: Email claim is missing.");

            var email = claims["email"];

            var session = await _client.Auth.SignIn(email, request.OldPassword);
            if (session == null)
            {
                throw new Exception("Invalid current password or email.");
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                throw new ArgumentException("Password fields cannot be null or empty.");
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                throw new Exception("Password is not valid: password and confirm password are not the same.");
            }

            if (!PasswordValidator.ValidatePassword(request.NewPassword))
            {
                throw new Exception("Password is not valid: password must contain at least one lowercase, uppercase letter, digit and special character.");
            }

            await _client.Auth.Update(new UserAttributes { Password = request.NewPassword });

        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }
}


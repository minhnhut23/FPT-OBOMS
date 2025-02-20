using AuthService.Utils;
using BusinessObject.DTO;
using Supabase.Gotrue;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


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
            var session = await _client.Auth.SignInWithPassword(email, password);

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
            var options = new SignInWithPasswordlessEmailOptions(email);
            await _client.Auth.SignInWithOtp(options);
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

            using var httpClient = new HttpClient();
            var requestUrl = "https://cnbwnwbtafbarsgmcabf.supabase.co/auth/v1/verify";

            httpClient.DefaultRequestHeaders.Add("apikey", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImNuYndud2J0YWZiYXJzZ21jYWJmIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzE0MDM3NjEsImV4cCI6MjA0Njk3OTc2MX0.mNGsOKRoaTQdB7fG8OJiddqslin08Yvx3uR13hDFNAA");

            var requestBody = new
            {
                email = request.Email,
                token = request.Token,
                type = "email"
            };

            string jsonPayload = JsonSerializer.Serialize(requestBody);
            var jsonContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(requestUrl, jsonContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(responseBody));
            }

            using var doc = JsonDocument.Parse(responseBody);
            string? accessToken = doc.RootElement.GetProperty("access_token").GetString();

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Failed to verify OTP. No access token received.");
            }

            var updateUrl = "https://cnbwnwbtafbarsgmcabf.supabase.co/auth/v1/user";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var updateBody = new { password = request.NewPassword };
            string updatePayload = JsonSerializer.Serialize(updateBody);
            var updateContent = new StringContent(updatePayload, Encoding.UTF8, "application/json");

            var updateResponse = await httpClient.PutAsync(updateUrl, updateContent);
            var updateResponseBody = await updateResponse.Content.ReadAsStringAsync();

            if (!updateResponse.IsSuccessStatusCode)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(updateResponseBody));
            }
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


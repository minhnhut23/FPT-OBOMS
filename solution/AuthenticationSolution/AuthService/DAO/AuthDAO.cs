using BusinessObject.DTO;
using Newtonsoft.Json;
using Supabase.Gotrue;

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

            return new LoginResponseDTO
            {
                AccessToken = session!.AccessToken!
            };

        }
        catch (Exception ex)
        {
            string errorMessage = ex.Message;

            if (IsJson(errorMessage))
            {
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                    errorMessage = errorObj?.msg ?? "An unknown error occurred";
                }
                catch (JsonException)
                {
                    errorMessage = "Error processing the custom error message.";
                }
            }

            throw new Exception(errorMessage);
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

            if (!ValidatePassword(request.Password))
            {
                throw new Exception("Password is not valid: password must contain at least one lowercase, uppercase letter, digit and special character.");
            }

            var session = await _client.Auth.SignUp(request.Email, request.Password);
        }
        catch (Exception ex)
        {
            string errorMessage = ex.Message;

            if (IsJson(errorMessage))
            {
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                    errorMessage = errorObj?.msg ?? "An unknown error occurred";
                }
                catch (JsonException)
                {
                    errorMessage = "Error processing the custom error message.";
                }
            }

            throw new Exception(errorMessage);
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
            throw new Exception(ex.Message);
        }
    }


    public async Task<User> GetUser()
    {
        var checksession = _client.Auth.CurrentUser;

        return checksession;
    }

    public static bool IsJson(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        input = input.Trim();

        if ((input.StartsWith("{") && input.EndsWith("}")) || 
            (input.StartsWith("[") && input.EndsWith("]")))
        {
            try
            {
                JsonConvert.DeserializeObject(input);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        return false;
    }

    private bool ValidatePassword(string passWord)
    {
        int validConditions = 0;
        foreach (char c in passWord)
        {
            if (c >= 'a' && c <= 'z')
            {
                validConditions++;
                break;
            }
        }
        foreach (char c in passWord)
        {
            if (c >= 'A' && c <= 'Z')
            {
                validConditions++;
                break;
            }
        }
        if (validConditions == 0) return false;
        foreach (char c in passWord)
        {
            if (c >= '0' && c <= '8')
            {
                validConditions++;
                break;
            }
        }
        if (validConditions == 1) return false;
        if (validConditions == 2)
        {
            char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' };   
            if (passWord.IndexOfAny(special) == -1) return false;
        }
        return true;
    }

}


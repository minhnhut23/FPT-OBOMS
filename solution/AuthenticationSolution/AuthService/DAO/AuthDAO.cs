using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supabase;
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
                throw new Exception("Password and Confirm Password are not the same");
            }

            if (!ValidatePassword(request.Password))
            {
                throw new Exception("Password is not valid");
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
            if (c >= '0' && c <= '9')
            {
                validConditions++;
                break;
            }
        }
        if (validConditions == 1) return false;
        if (validConditions == 2)
        {
            char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever    
            if (passWord.IndexOfAny(special) == -1) return false;
        }
        return true;
    }

}


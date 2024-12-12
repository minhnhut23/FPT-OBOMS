using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
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
            throw new Exception(ex.Message);
        }
    }

    public async Task Register(RegisterRequestDTO request)
    {
        try
        {
            var session = await _client.Auth.SignUp(request.Email, request.Password);

        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }   
    }

}


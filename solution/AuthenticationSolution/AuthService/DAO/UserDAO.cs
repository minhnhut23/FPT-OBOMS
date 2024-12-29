using AuthService.Utils;
using BusinessObject.DTO;
using BusinessObject.Models;
using Supabase;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.DAO;

public class UserDAO
{
    private readonly Supabase.Client _client;

    public UserDAO(Client client)
    {
        _client = client;
    }

    public async Task<GetUserResponeDTO> GetCurrentUser(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

            var accountId = Guid.Parse(claims["sub"]);

            var profile = await _client
                .From<Profile>()
                .Where(x => x.AccountId == accountId)
                .Single();

            if (profile == null)
            {
                throw new Exception("You have not update your account yet!");
            }

            return new GetUserResponeDTO
            {
                AccountId = accountId,
                Email = claims["email"],
                FullName = profile.FullName,
                Bio = profile.Bio!,
                DateOfBirth = profile.DateOfBirth,
                ProfilePicture = profile.ProfilePicture!
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }

    }

    public async Task<Profile> CreateUser(CreateProfileRequestDTO request, string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            var accountId = Guid.Parse(claims["sub"]);

            var profile = await _client
                .From<Profile>()
                .Where(x => x.AccountId == accountId)
                .Single();

            if (profile != null)
            {
                throw new Exception("The user already exists!");
            }

            var result = new Profile
            {
                FullName = request.FullName,
                ProfilePicture = request.ProfilePicture,
                Bio = request.Bio,
                Role = request.Role,
                DateOfBirth = request.DateOfBirth,
                AccountId = accountId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _client.From<Profile>().Insert(result);

            return result;

        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }
}

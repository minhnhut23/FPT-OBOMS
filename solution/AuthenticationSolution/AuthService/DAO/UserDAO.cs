using AuthService.Utils;
using BusinessObject.DTO;
using BusinessObject.Models;
using Supabase.Gotrue;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.DAO;

public class UserDAO
{
    private readonly Supabase.Client _client;

    public UserDAO(Supabase.Client client)
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
                return new GetUserResponeDTO
                {
                    Email = claims["email"]
                };
            }
            else
            {
                return new GetUserResponeDTO
                {
                    Email = claims["email"],
                    FullName = profile.FullName,
                    Bio = profile.Bio!,
                    DateOfBirth = profile.DateOfBirth,
                    ProfilePicture = profile.ProfilePicture!
                };
            }
            
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }

    }
     public async Task<GetUserResponeDTO> GetUserById(Guid profileId)
    {
        try
        {          
            var profile = await _client
                .From<Profile>()
                .Where(x => x.Id == profileId)
                .Single();

            if (profile == null)
            {
               throw new Exception("Profile not found!");
            }
            else
            {
                return new GetUserResponeDTO
                {                   
                    FullName = profile.FullName,
                    Bio = profile.Bio!,
                    DateOfBirth = profile.DateOfBirth,
                    ProfilePicture = profile.ProfilePicture!
                };
            }
            
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }

    }
    
    public async Task<GetUserResponeDTO> UpdateProfile(UpdateProfileRequestDTO request, string token)
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
               throw new Exception("Profile not found!");
            }
            else
            {

                if (!DateOfBirthValidator.IsValid(request.DateOfBirth))
                {
                    throw new Exception("Invalid Birthday!");
                }

                profile.ProfilePicture = request.ProfilePicture;
                profile.FullName = request.FullName;
                profile.Bio = request.Bio; 
                profile.DateOfBirth = request.DateOfBirth;
                profile.UpdatedAt = DateTime.UtcNow;
                await profile.Update<Profile>();

                if (request.Email != null && request.Email != claims["email"])
                {
                    var attrs = new UserAttributes { Email = request.Email };
                    await _client.Auth.Update(attrs);

                    return new GetUserResponeDTO
                    {
                        Email = request.Email,
                        FullName = profile.FullName,
                        Bio = profile.Bio!,
                        DateOfBirth = profile.DateOfBirth,
                        ProfilePicture = profile.ProfilePicture!
                    };
                }

                return new GetUserResponeDTO
                {
                    Email = claims["email"],
                    FullName = profile.FullName,
                    Bio = profile.Bio!,
                    DateOfBirth = profile.DateOfBirth,
                    ProfilePicture = profile.ProfilePicture!
                };
            }
            
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }    


}

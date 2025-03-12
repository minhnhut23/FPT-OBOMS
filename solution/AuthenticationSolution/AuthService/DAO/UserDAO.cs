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

            var accountId = Guid.Parse(claims["sub"].Trim());

            var profile = await _client
                .From<Profile>()
                .Where(x => x.AccountId == accountId)
                .Single();

            if (profile == null)
            {
                throw new Exception("Profile not found! ==== " + accountId);
            }

            if (request.ProfilePicture != null)
            {
                var file = request.ProfilePicture;
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var localPath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await file.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(profile.ProfilePicture))
                {
                    var oldFileName = profile.ProfilePicture.Split('/').Last();
                    await _client.Storage.From("avatars").Remove(new List<string> { oldFileName });
                }

                await _client.Storage.From("avatars").Upload(localPath, fileName, new Supabase.Storage.FileOptions { Upsert = true });

                profile.ProfilePicture = _client.Storage.From("avatars").GetPublicUrl(fileName);

                if (File.Exists(localPath))
                {
                    File.Delete(localPath);
                }
            }

            profile.FullName = request.FullName ?? profile.FullName;
            profile.Bio = request.Bio ?? profile.Bio;
            profile.DateOfBirth = request.DateOfBirth != default ? request.DateOfBirth : profile.DateOfBirth;
            profile.UpdatedAt = DateTime.UtcNow;

            await profile.Update<Profile>();

            if (request.Email != null && request.Email != claims["email"])
            {
                var attrs = new UserAttributes { Email = request.Email };
                await _client.Auth.Update(attrs);
            }

            return new GetUserResponeDTO
            {
                Email = claims["email"],
                FullName = profile.FullName!,
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
}

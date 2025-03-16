using AuthService.Utils;
using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Identity;
using Supabase.Gotrue;
using System.IdentityModel.Tokens.Jwt;
using static Supabase.Postgrest.Constants;

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
                throw new Exception("Profile not found!");
            }

            if (request.ProfilePicture != null)
            {
                if (!ImageValidator.IsValidImage(request.ProfilePicture, out string error))
                {
                    throw new Exception(error);
                }

                var file = request.ProfilePicture;
                var fileName = $"{Guid.NewGuid()}_{file.FileName.Trim()}";

                if (!string.IsNullOrEmpty(profile.ProfilePicture))
                {
                    var oldFileName = Path.GetFileName(new Uri(profile.ProfilePicture).AbsolutePath);
                    var deleteResult = await _client.Storage.From("avatars").Remove(new List<string> { oldFileName });                   
                }

                byte[] fileBytes;
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    fileBytes = stream.ToArray();
                }

                await _client.Storage
                    .From("avatars")
                    .Upload(fileBytes, fileName, new Supabase.Storage.FileOptions { CacheControl = "3600", Upsert = false });

                var publicUrl = _client.Storage.From("avatars").GetPublicUrl(fileName);

                profile.ProfilePicture = publicUrl;
            }

            if (!DateOfBirthValidator.IsValid(request.DateOfBirth))
            {
                throw new Exception("Invalid Birthday!");
            }

            profile.FullName = request.FullName ?? profile.FullName;
            profile.Bio = request.Bio ?? profile.Bio;
            profile.DateOfBirth = request.DateOfBirth != default ? request.DateOfBirth : profile.DateOfBirth;
            profile.UpdatedAt = DateTime.UtcNow;

            await profile.Update<Profile>();

            string emailResponse = claims["email"];

            if (request.Email != null && request.Email != claims["email"])
            {
                var attrs = new UserAttributes { Email = request.Email };
                var user = await _client.Auth.Update(attrs);
                emailResponse = user!.Email!;
            }            

            return new GetUserResponeDTO
            {
                Email = emailResponse,
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

    public async Task<(List<Profile> Profiles, PaginationDTO PaginationMetadata)> GetAllProfiles(GetProfileRequestDTO request)
    {
        try
        {
            var query = _client.From<Profile>().Select("*");
            query = ApplyGetAllFilters.ApplyProfileFilters(query, request);

            var counting = _client.From<Profile>().Select("*");
            counting = ApplyGetAllFilters.ApplyProfileFilters(counting, request);
            var totalRecordsResponse = await counting.Select("id").Get();
            var totalRecords = totalRecordsResponse.Models?.Count ?? 0;
            var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedQuery = query.Range(skip, skip + request.PageSize - 1);

            var profilesResponse = await paginatedQuery.Get();
            var profiles = profilesResponse.Models.ToList();

            if (totalRecords == 0 || request.PageNumber > totalPages)
            {
                return (
                    new List<Profile>(),
                    new PaginationDTO
                    {
                        TotalResults = totalRecords,
                        TotalPages = totalPages,
                        CurrentPage = request.PageNumber,
                        PageSize = request.PageSize
                    }
                );
            }

            var paginationMetadata = new PaginationDTO
            {
                TotalResults = totalRecords,
                TotalPages = totalPages,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize
            };

            return (profiles, paginationMetadata);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching profiles: {ex.Message}");
        }
    }
}

using BusinessObject.DTO;
using BusinessObject.Models;

namespace AuthService.IRepositories;

public interface IUserRepository
{
    Task<GetUserResponeDTO> GetCurrentUser(string token);

    Task<GetUserResponeDTO> GetUserById(Guid profileId);

    Task<GetUserResponeDTO> UpdateProfile(UpdateProfileRequestDTO request, string token);

    Task<(List<Profile> Profiles, PaginationDTO PaginationMetadata)> GetAllProfiles(GetProfileRequestDTO request);

}

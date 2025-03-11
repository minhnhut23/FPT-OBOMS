using BusinessObject.DTO;

namespace AuthService.IRepositories;

public interface IUserRepository
{
    public Task<GetUserResponeDTO> GetCurrentUser(string token);

    public Task<GetUserResponeDTO> GetUserById(Guid profileId);

    public Task<GetUserResponeDTO> UpdateProfile(UpdateProfileRequestDTO request, string token);

}

using AuthService.DAO;
using AuthService.IRepositories;
using BusinessObject.DTO;
using BusinessObject.Models;

namespace AuthService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDAO _dao;

    public UserRepository(UserDAO dao)
    {
        _dao = dao;
    }

    public Task<(List<Profile> Profiles, PaginationDTO PaginationMetadata)> GetAllProfiles(GetProfileRequestDTO request) => _dao.GetAllProfiles(request);

    public Task<GetUserResponeDTO> GetCurrentUser(string token) => _dao.GetCurrentUser(token);

    public Task<GetUserResponeDTO> GetUserById(Guid profileId) => _dao.GetUserById(profileId);

    public Task<GetUserResponeDTO> UpdateProfile(UpdateProfileRequestDTO request, string token) => _dao.UpdateProfile(request, token);
}

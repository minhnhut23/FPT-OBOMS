using AuthService.DAO;
using AuthService.Interfaces.Repositories;
using BusinessObject.DTO;

namespace AuthService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDAO _dao;

    public UserRepository(UserDAO dao)
    {
        _dao = dao;
    }

    public Task<GetUserResponeDTO> CreateUser(CreateProfileRequestDTO request, string token) => _dao.CreateUser(request, token);

    public Task<GetUserResponeDTO> GetCurrentUser(string token) => _dao.GetCurrentUser(token);

    public Task<GetUserResponeDTO> GetUserById(Guid profileId) => _dao.GetUserById(profileId);

    public Task<GetUserResponeDTO> UpdateProfile(UpdateProfileRequestDTO request, string token) => _dao.UpdateProfile(request, token);
}

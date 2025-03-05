using AuthService.DAO;
using AuthService.IRepositories;
using BusinessObject.DTO;

namespace AuthService.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AuthDAO _dao;

    public AuthRepository(AuthDAO dao)
    {
        _dao = dao;
    }

    public Task ChangePassword(ChangePasswordRequestDTO request, string token) => _dao.ChangePassword(request, token);

    public Task ForgotPassword(string email) => _dao.ForgotPassword(email);

    public Task<LoginResponseDTO> Login(string email, string password) => _dao.Login(email, password);

    public Task Logout() => _dao.Logout();

    public Task<LoginResponseDTO> RefreshToken(string refreshToken) => _dao.RefreshToken(refreshToken);

    public Task<GetUserResponeDTO> Register(RegisterRequestDTO request) => _dao.Register(request);

    public Task ResetPassword(RecoverPasswordRequestDTO request) => _dao.ResetPassword(request);
}

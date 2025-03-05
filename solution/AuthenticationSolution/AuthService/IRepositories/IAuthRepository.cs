using BusinessObject.DTO;

namespace AuthService.IRepositories;

public interface IAuthRepository
{
    Task<LoginResponseDTO> Login(string email, string password);

    Task<GetUserResponeDTO> Register(RegisterRequestDTO request);

    Task Logout();

    Task ChangePassword(ChangePasswordRequestDTO request, string token);

    Task ResetPassword(RecoverPasswordRequestDTO request);

    Task ForgotPassword(string email);

    Task<LoginResponseDTO> RefreshToken(string refreshToken);

}

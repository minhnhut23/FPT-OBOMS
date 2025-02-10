using BusinessObject.DTO;

namespace AuthService.IRepositories;

public interface IAuthRepository
{
    public Task<LoginResponseDTO> Login(string email, string password);

    public Task Register(RegisterRequestDTO request);

    public Task Logout();

    public Task ChangePassword(ChangePasswordRequestDTO request, string token);

    public Task ResetPassword(RecoverPasswordRequestDTO request);

    public Task ForgotPassword(string email);

}

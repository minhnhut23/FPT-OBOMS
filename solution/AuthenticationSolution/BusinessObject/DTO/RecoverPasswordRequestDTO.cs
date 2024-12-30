namespace BusinessObject.DTO;

public class RecoverPasswordRequestDTO
{
    public string AccessToken { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}

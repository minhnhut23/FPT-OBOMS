using Supabase.Gotrue;

namespace BusinessObject.DTO;

public class LoginResponseDTO
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}

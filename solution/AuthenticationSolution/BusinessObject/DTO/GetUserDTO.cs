using static BusinessObject.Models.Profile;

namespace BusinessObject.DTO;

public class GetUserDTO
{
    public Guid AccountId { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public Roles Role { get; set; }

}

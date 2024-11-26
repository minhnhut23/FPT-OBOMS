using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models;

[Table("owners")]
public class Owners : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }
    [Column("username")]
    public string Username { get; set; } = null!;
    [Column("password")]
    public string Password { get; set; } = null!;
    [Column("email")]
    public string Email { get; set; } = null!;
    [Column("phone_number")]
    public string PhoneNumber { get; set; } = null!;
    [Column("profile_picture")]
    public string Profile_Picture { get; set; } = null!;
    [Column("bio")]
    public string Bio {  get; set; } = null!;
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models;

[Table("profile")]
public class Profile : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("profile_picture")]
    public string ProfilePicture { get; set; } = null!;

    [Column("bio")]
    public string Bio { get; set; } = null!;

    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("role")]
    public Roles Role { get; set; } 

    public enum Roles
    {
        Customer,
        Owner,
        Admin
    } 
}

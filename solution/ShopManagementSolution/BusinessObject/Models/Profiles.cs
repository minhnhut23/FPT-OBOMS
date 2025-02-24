using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enums;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models
{
    [Table("profiles")]
    public class Profiles : BaseModel
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("profile_picture")]
        public string? ProfilePicture { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("account_id")]
        public Guid AccountId { get; set; }

        [Required]
        [Column("role")]
        public UserRole Role { get; set; }

        [Required]
        [Column("full_name")]
        public string FullName { get; set; }
    }
}

using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models;

[Table("shops")]
public class Shop : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("owner_id")]
    public Guid OwnerId { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("address")]
    public string Address { get; set; } = null!;

    [Column("phone_number")]
    public string PhoneNumber { get; set; } = null!;

    [Column("opening_hours")]
    public string OpeningHours { get; set; } = null!;

    [Column("closing_hours")]
    public string ClosingHours { get; set; } = null!;

    [Column("rating")]
    public decimal Rating { get; set; }

    [Column("cuisine_type")]
    public string CuisineType { get; set; } = null!;

    [Column("latitude")]
    public decimal Latitude { get; set; }

    [Column("cuisine_type")]
    public decimal Longitude { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public Owner Owners { get; set; } = null!;
}

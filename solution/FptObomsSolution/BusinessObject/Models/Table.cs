using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models;

[Table("tables")]
public class Table : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("shop_id")]
    public Guid ShopId { get; set; }

    [Column("table_number")]
    public string TableNumber { get; set; } = null!;

    [Column("capacity")]
    public string Capacity { get; set; } = null!;

    [Column("status")]
    public string Status { get; set; } = null!;

    [Column("location_description")]
    public string LocationDescription { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public Shop Shops { get; set; } = null!;
}

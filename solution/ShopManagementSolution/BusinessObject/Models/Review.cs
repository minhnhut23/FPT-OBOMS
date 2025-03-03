using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models;

[Table("reviews")]
public class Review : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("customer_id")]
    public Guid CustomerId { get; set; }
    [Column("shop_id")]
    public Guid ShopId { get; set; }

    [Column("rating")]
    public int Rating { get; set; }

    [Column("comment")]
    public string Comment { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

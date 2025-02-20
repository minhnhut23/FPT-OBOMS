using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models;

public class ShopSubscriptions : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("started_at")]
    public DateTime StartedAt { get; set; }

    [Column("ended_at")]
    public DateTime EndedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("shop_id")]
    public Guid ShopId { get; set; }

    [Column("supcription_id")]
    public Guid SubscriptionId { get; set; }
}

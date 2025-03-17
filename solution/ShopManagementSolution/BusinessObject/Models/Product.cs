using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models;
[Table("products")]
public class Product : BaseModel
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("shop_id")]
    public Guid ShopId { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("price")]
    public decimal Price { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("category")]
    public string Category { get; set; } = null!;

    [Column("is_available")]
    public bool IsAvailable { get; set; }

    [Column("ingredients")]
    public string Ingredient { get; set; } = null!;

    [Column("nutritional_info")]
    public string NutritionalIfo { get; set; } = null!;

    [Column("image")]
    public string Image { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

}


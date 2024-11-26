using Supabase.Postgrest.Attributes;

namespace BusinessObject.Models;

[Table("menu_items")]
public class MenuItem
{
    [PrimaryKey("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("price")]
    public decimal Price { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("category")]
    public string Category { get; set; } = null!;

    [Column("is_available")]
    public string IsAvailable { get; set; } = null!;

    [Column("ingredients")]
    public string Ingredients { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public Shop Shops { get; set; } = null!;

}

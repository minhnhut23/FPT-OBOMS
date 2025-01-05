using System;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models
{
    [Table("bills_details")]
    public class BillDetail: BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("bills_id")]
        [Required(ErrorMessage = "Bill ID is required.")]
        public Guid BillId { get; set; }

        [Column("menu_item_id")]
        [Required(ErrorMessage = "Menu item ID is required.")]
        public Guid MenuItemId { get; set; }

        [Column("quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Column("price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}

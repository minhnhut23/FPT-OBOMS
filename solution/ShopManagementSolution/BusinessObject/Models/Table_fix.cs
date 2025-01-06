using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;


namespace BusinessObject.Models
{
    [Table("tables")]
    public class Table_fix : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }
        [Column("table_number")]
        [Required]
        public string TableNumber { get; set; }
       
        [Column("capacity")]
        [Required]
        public int Capacity { get; set; }

        [Column("status")]
        [Required]
        public string Status { get; set; }
        [Column("location_description")]
        public string LocationDescription { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("shop_id")]
        [Required]
        public Guid ShopId { get; set; }

        [Required]
        [Column("table_type")]
        public string TableType { get; set; }

    }
}

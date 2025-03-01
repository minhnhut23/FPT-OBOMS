using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    [Table("table_types")]
    public class TableType : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("price_by_hour")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal? PriceByHour { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

}

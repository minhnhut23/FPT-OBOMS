using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models
{
    [Table("billiard_table")]
    public class BilliardTable: BaseModel
    {
        [PrimaryKey("table_id")]
        public Guid TableId { get; set; }

        [Column("billiard_type")]
        public decimal BilliardType { get; set; }
        
        [Column("length")]
        public decimal Length { get; set; }

        [Column("width")]
        public decimal Width { get; set; }

        [Column("cloth_material")]
        public string ClothMaterial { get; set; }

        [Column("frame_material")]
        public string FrameMaterial { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
    }
}

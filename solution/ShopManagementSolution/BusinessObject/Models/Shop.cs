using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models
{
    [Table("shops")]
    public class Shop : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("opening_hours")]
        public TimeSpan OpeningHours { get; set; }

        [Column("closing_hours")]
        public TimeSpan ClosingHours { get; set; }

        [Column("rating")]
        public decimal? Rating { get; set; }

        [Column("latitude")]
        public decimal? Latitude { get; set; }

        [Column("longitude")]
        public decimal? Longitude { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
       
        [Column("owner_id")]
        public Guid OwnerId { get; set; }
    }
}

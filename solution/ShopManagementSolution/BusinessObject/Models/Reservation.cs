using BusinessObject.Enums;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BusinessObject.Models
{
    [Table("reservations")]
    public class Reservation : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("customer_id")]
        [Required]
        public Guid CustomerId { get; set; }

        [Column("shop_id")]
        [Required]
        public Guid ShopId { get; set; }

        [Column("table_id")]
        [Required]
        public Guid TableId { get; set; }

        [Column("reservation_time")]
        [Required]
        public DateTime ReservationTime { get; set; }

        [Column("status")]
        [Required]
        public string Status { get; set; }

        [IgnoreDataMember]
        public Enum_ReservationStatus StatusEnum
        {
            get => Enum.Parse<Enum_ReservationStatus>(Status, true);
            set => Status = value.ToString(); 
        }


        [Column("special_requests")]
        public string SpecialRequests { get; set; }

        [Column("number_of_guests")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be at least 1.")]
        public int NumberOfGuests { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

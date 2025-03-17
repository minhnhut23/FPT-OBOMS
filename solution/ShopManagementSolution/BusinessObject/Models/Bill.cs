
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BusinessObject.DTOs.BillDTO;
using BusinessObject.Enums;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models
{
    [Table("bills")]
    public class Bill : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("reservation_id")]
        [Required(ErrorMessage = "Reservation ID is required.")]
        public Guid? ReservationId { get; set; }

        [Column("total_amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a positive value.")]
        public decimal TotalAmount { get; set; } = 0m;

        [Column("received_amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Received amount must be a positive value.")]
        public decimal ReceivedAmount { get; set; }

        [Column("change_amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Change amount must be a positive value.")]
        public decimal ChangeAmount { get; set; }

        [Column("table_id")]
        public Guid TableId { get; set; }

        [Column("status")]
        [Required(ErrorMessage = "Status is required!")]
        public string Status { get; set; } = Enum_BillStatus.Pending.ToString();

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("customer_id")]
        public Guid? CustomerId { get; set; }

        [Column("shop_id")]
        [Required(ErrorMessage = "Shop ID is required.")]
        public Guid ShopId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BusinessObject.Enums;

namespace BusinessObject.Models
{
    [Table("payments")]
    public class Payment : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("shop_id")]
        [Required(ErrorMessage = "Shop ID is required.")]
        public Guid ShopId { get; set; }

        [Column("bill_id")]
        [Required(ErrorMessage = "Bill ID is required.")]
        public Guid BillId { get; set; }

        [Column("transaction_id")]
        public long? TransactionId { get; set; }

        [Column("method")]
        [Required(ErrorMessage = "Payment method is required.")]
        public string Method { get; set; } = Enum_PaymentMethod.Cash.ToString();

        [Column("amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; set; }

        [Column("received_amount")]
        public decimal? ReceivedAmount { get; set; } // Số tiền khách đưa

        [Column("change_amount")]
        public decimal? ChangeAmount { get; set; } // Tiền thối lại

        [Column("status")]
        [Required(ErrorMessage = "Payment status is required.")]
        public string Status { get; set; } = Enum_PaymentStatus.Pending.ToString();

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

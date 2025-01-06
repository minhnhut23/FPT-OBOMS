using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BusinessObject.Models
{
    [Table("bills")]
    public class Bill: BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("reservation_id")]
        [Required(ErrorMessage = "Reservation ID is required.")]
        public Guid ReservationId { get; set; }

        [Column("total_amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a positive value.")]
        public decimal TotalAmount { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("customer_id")]
        [Required(ErrorMessage = "Customer ID is required.")]
        public Guid CustomerId { get; set; }

        [Column("shop_id")]
        [Required(ErrorMessage = "Shop ID is required.")]
        public Guid ShopId { get; set; }
    }
}

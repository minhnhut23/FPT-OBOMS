using System;

namespace BusinessObject.DTOs.BillDTO
{
    public class BillResponseDTO
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceivedAmount { get; set; } // Tiền nhận
        public decimal ChangeAmount { get; set; } // Tiền trả lại
        public Guid TableId { get; set; } // ID bàn
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ShopId { get; set; }
        public int BillDetailsQuantity { get; set; }
    }
}

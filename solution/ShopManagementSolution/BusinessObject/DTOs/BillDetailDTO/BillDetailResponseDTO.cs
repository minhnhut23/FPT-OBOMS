using System;

namespace BusinessObject.DTOs.BillDetailDTO
{
    public class BillDetailResponseDTO
    {
        public Guid Id { get; set; }
        public Guid BillId { get; set; }
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

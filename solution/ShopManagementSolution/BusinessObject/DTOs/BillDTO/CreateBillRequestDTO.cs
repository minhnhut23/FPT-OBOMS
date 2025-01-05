using System;

namespace BusinessObject.DTOs.BillDTO
{
    public class CreateBillRequestDTO
    {
        public Guid ReservationId { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ShopId { get; set; }
    }
}

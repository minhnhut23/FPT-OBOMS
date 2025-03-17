namespace BusinessObject.DTOs.PaymentDTO
{
    public class PaymentRequestDTO
    {
        public Guid ShopId { get; set; }
        public Guid BillId { get; set; }
        public string Method { get; set; } // "Cash" hoặc "MoMo"
        public decimal Amount { get; set; }
        public decimal? ReceivedAmount { get; set; } // Tiền khách đưa (chỉ dùng khi thanh toán tiền mặt)
    }
}

namespace BusinessObject.DTOs.PaymentDTO
{
    public class PaymentResponseDTO
    {
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public Guid BillId { get; set; }
        public long? TransactionId { get; set; }
        public string Method { get; set; }
        public decimal Amount { get; set; }
        public decimal? ReceivedAmount { get; set; } // Tiền khách đưa
        public decimal? ChangeAmount { get; set; } // Tiền thối
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

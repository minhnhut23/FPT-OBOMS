namespace BusinessObject.DTOs.PaymentDTO
{
    public class PaymentResponseStatusDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public PaymentResponseDTO? Data { get; set; }
    }
}

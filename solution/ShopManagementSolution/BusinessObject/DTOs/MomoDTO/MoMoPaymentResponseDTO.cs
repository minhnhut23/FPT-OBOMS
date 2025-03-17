namespace BusinessObject.DTOs.PaymentDTO
{
    public class MoMoPaymentResponseDTO
    {
        public string PartnerCode { get; set; }
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public int Amount { get; set; }
        public string PayUrl { get; set; }  // Link QR Code để khách thanh toán
        public int ResultCode { get; set; }  // 0 = Thành công, khác 0 = Thất bại
        public string Message { get; set; }
    }
}

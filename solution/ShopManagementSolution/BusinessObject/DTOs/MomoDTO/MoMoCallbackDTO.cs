namespace BusinessObject.DTOs.PaymentDTO
{
    public class MoMoCallbackDTO
    {
        public string PartnerCode { get; set; }
        public Guid OrderId { get; set; }  // Trùng với orderId ban đầu gửi đi
        public Guid RequestId { get; set; }
        public int Amount { get; set; }
        public int ResultCode { get; set; }  // 0 = Thành công, khác 0 = Thất bại
        public string Message { get; set; }
    }
}

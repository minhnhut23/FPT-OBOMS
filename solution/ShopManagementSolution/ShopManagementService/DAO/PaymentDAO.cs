using BusinessObject.DTOs.PaymentDTO;
using BusinessObject.Models;
using Supabase;
using System;
using System.Threading.Tasks;

namespace ShopManagementService.DAO
{
    public class PaymentDAO
    {
        private readonly Client _client;

        public PaymentDAO(Client client)
        {
            _client = client;
        }
        public async Task<PaymentResponseDTO> GetPaymentByOrderId(Guid orderId)
        {
            try
            {
                var payments = await _client
                    .From<Payment>()
                    .Where(p => p.BillId == orderId)
                    .Get();

                var payment = payments.Models.FirstOrDefault();

                if (payment == null)
                {
                    return null;
                }

                return new PaymentResponseDTO
                {
                    Id = payment.Id,
                    ShopId = payment.ShopId,
                    BillId = payment.BillId,
                    Method = payment.Method,
                    Amount = payment.Amount,
                    ReceivedAmount = payment.ReceivedAmount,
                    ChangeAmount = payment.ChangeAmount,
                    Status = payment.Status,
                    CreatedAt = payment.CreatedAt,
                    UpdatedAt = payment.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin thanh toán: " + ex.Message);
            }
        }
        public async Task<PaymentResponseDTO> CreatePayment(PaymentRequestDTO request)
        {
            try
            {
                var newPayment = new Payment
                {
                    Id = Guid.NewGuid(),
                    ShopId = request.ShopId,
                    BillId = request.BillId,
                    Method = request.Method,
                    Amount = request.Amount,
                    ReceivedAmount = request.ReceivedAmount,
                    Status = "Paid",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                if (request.Method == "Cash")
                {
                    var changeAmount = request.ReceivedAmount - request.Amount;
                    if (changeAmount < 0)
                        throw new Exception("Số tiền nhận không đủ.");
                    newPayment.ChangeAmount = changeAmount;
                }

                await _client.From<Payment>().Insert(newPayment);

                return new PaymentResponseDTO
                {
                    Id = newPayment.Id,
                    ShopId = newPayment.ShopId,
                    BillId = newPayment.BillId,
                    Method = newPayment.Method,
                    Amount = newPayment.Amount,
                    ReceivedAmount = newPayment.ReceivedAmount,
                    ChangeAmount = newPayment.ChangeAmount,
                    Status = newPayment.Status,
                    CreatedAt = newPayment.CreatedAt,
                    UpdatedAt = newPayment.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lưu thanh toán: " + ex.Message);
            }
        }
    }

}

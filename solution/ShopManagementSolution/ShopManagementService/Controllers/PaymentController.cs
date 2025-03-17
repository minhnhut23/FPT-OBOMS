using BusinessObject.DTOs.PaymentDTO;
using BusinessObject.Enums;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using ShopManagementService.DAO;
using ShopManagementService.IRepositories;
using System;
using System.Threading.Tasks;

namespace ShopManagementService.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentDAO _paymentDao;
        private readonly MoMoService _moMoService;

        public PaymentController(PaymentDAO paymentDao, MoMoService moMoService)
        {
            _paymentDao = paymentDao;
            _moMoService = moMoService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDTO request)
        {
            try
            {
                if (request.Method == Enum_PaymentMethod.MoMo.ToString())
                {
                    var moMoResponse = await _moMoService.CreateMoMoPayment(request);
                    if (moMoResponse?.ResultCode != 0)
                    {
                        return BadRequest(new PaymentResponseStatusDTO
                        {
                            Success = false,
                            Message = "Thanh toán MoMo thất bại.",
                            Data = null
                        });
                    }
                    return Ok(new { QrCodeUrl = moMoResponse.PayUrl });
                }
                else if (request.Method == Enum_PaymentMethod.Cash.ToString())
                {
                    var payment = await _paymentDao.CreatePayment(request);
                    return Ok(new PaymentResponseStatusDTO
                    {
                        Success = true,
                        Message = "Thanh toán tiền mặt thành công.",
                        Data = payment
                    });
                }
                return BadRequest("Phương thức thanh toán không hợp lệ.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("momo-callback")]
        public async Task<IActionResult> MoMoCallback([FromBody] MoMoCallbackDTO callback)
        {
            Console.WriteLine("🔹 Nhận request từ MoMo callback");

            if (callback == null)
            {
                Console.WriteLine("❌ Dữ liệu callback không hợp lệ.");
                return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ." });
            }

            Console.WriteLine($"🔹 Kiểm tra giao dịch với OrderId: {callback.OrderId}");

            // Lấy giao dịch từ DB để kiểm tra xem đã lưu chưa
            var existingPayment = await _paymentDao.GetPaymentByOrderId(callback.OrderId);

            if (existingPayment != null)
            {
                Console.WriteLine("✅ Giao dịch đã tồn tại trong database.");
                return Ok(new { Success = true, Message = "Giao dịch đã được xử lý." });
            }

            // Kiểm tra kết quả giao dịch
            bool isSuccess = callback.ResultCode == 0;
            Console.WriteLine(isSuccess ? "✅ Thanh toán thành công!" : "❌ Thanh toán thất bại.");

            var paymentRequest = new PaymentRequestDTO
            {
                BillId = callback.OrderId,
                Amount = callback.Amount,
                Method = "MoMo",
                ReceivedAmount = isSuccess ? callback.Amount : 0,  // Nếu thất bại, không ghi nhận số tiền
            };

            Console.WriteLine("🔹 Tiến hành lưu giao dịch vào database...");

            // Lưu giao dịch vào database
            var payment = await _paymentDao.CreatePayment(paymentRequest);

            Console.WriteLine("✅ Giao dịch đã được lưu vào database.");

            return Ok(new
            {
                Success = isSuccess,
                Message = isSuccess ? "Thanh toán MoMo thành công." : "Thanh toán MoMo thất bại.",
                Data = payment
            });
        }


    }
}
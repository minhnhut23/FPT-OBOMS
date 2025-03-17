//using BusinessObject.DTOs.PaymentDTO;
//using BusinessObject.Services;
//using ShopManagementService.IRepositories;
//using ShopManagementService.DAO;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using BusinessObject.Models;

//namespace ShopManagementService.Repositories
//{
//    public class PaymentRepository : IPaymentRepository
//    {
//        public async Task<Payment> CreatePaymentAsync(Payment payment)
//        {
//            // Logic để lưu vào cơ sở dữ liệu
//            return await PaymentDAO.CreatePaymentAsync(payment);
//        }

//        public async Task<PaymentResponseDTO> GetPaymentByIdAsync(Guid id)
//        {
//            var payment = await PaymentDAO.GetPaymentByIdAsync(id);
//            return new PaymentResponseDTO
//            {
//                Id = payment.Id,
//                ShopId = payment.ShopId,
//                BillId = payment.BillId,
//                TransactionId = payment.TransactionId,
//                Method = payment.Method,
//                Amount = payment.Amount,
//                ReceivedAmount = payment.ReceivedAmount,
//                ChangeAmount = payment.ChangeAmount,
//                Status = payment.Status,
//                CreatedAt = payment.CreatedAt,
//                UpdatedAt = payment.UpdatedAt
//            };
//        }

//        public async Task<IEnumerable<PaymentResponseDTO>> GetAllPaymentsAsync()
//        {
//            var payments = await PaymentDAO.GetAllPaymentsAsync();
//            return payments.Select(p => new PaymentResponseDTO
//            {
//                Id = p.Id,
//                ShopId = p.ShopId,
//                BillId = p.BillId,
//                TransactionId = p.TransactionId,
//                Method = p.Method,
//                Amount = p.Amount,
//                ReceivedAmount = p.ReceivedAmount,
//                ChangeAmount = p.ChangeAmount,
//                Status = p.Status,
//                CreatedAt = p.CreatedAt,
//                UpdatedAt = p.UpdatedAt
//            });
//        }
//    }
//}
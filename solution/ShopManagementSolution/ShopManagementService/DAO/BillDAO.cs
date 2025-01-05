using BusinessObject.DTOs.BillDetailDTO;
using BusinessObject.DTOs.BillDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessObject.Services
{
    public class BillDAO
    {
        private readonly Supabase.Client _client;

        public BillDAO(Supabase.Client client)
        {
            _client = client;
        }

        public async Task<List<BillResponseDTO>> GetAllBills()
        {
            try
            {
                var billResponse = await _client.From<Bill>().Get();
                var bills = billResponse.Models;

                var billDetailsResponse = await _client.From<BillDetail>().Get();
                var billDetailsDict = billDetailsResponse.Models
                    .GroupBy(bd => bd.BillId)
                    .ToDictionary(g => g.Key, g => g.Sum(bd => bd.Quantity));
                var billResponseDTOs = bills.Select(bill => new BillResponseDTO
                {
                    Id = bill.Id,
                    ReservationId = bill.ReservationId,
                    TotalAmount = bill.TotalAmount,
                    CreatedAt = bill.CreatedAt,
                    UpdatedAt = bill.UpdatedAt,
                    CustomerId = bill.CustomerId,
                    ShopId = bill.ShopId,
                    BillDetailsQuantity = billDetailsDict.ContainsKey(bill.Id) ? billDetailsDict[bill.Id] : 0
                }).ToList();

                return billResponseDTOs;
            }
            catch (Exception ex)
            {
                // Log and throw the exception after processing the error message
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<BillWithDetailsResponseDTO?> GetBillById(Guid id)
        {
            try
            {
                var billResponse = await _client.From<Bill>().Where(b => b.Id == id).Single();
                if (billResponse == null)
                    return null;

                var billDetailsResponse = await _client.From<BillDetail>().Where(bd => bd.BillId == id).Get();

                var billDetailsDTOs = billDetailsResponse.Models.Select(bd => new BillDetailResponseDTO
                {
                    Id = bd.Id,
                    BillId = bd.BillId,
                    MenuItemId = bd.MenuItemId,
                    Quantity = bd.Quantity,
                    Price = bd.Price,
                    CreatedAt = bd.CreatedAt,
                    UpdatedAt = bd.UpdatedAt
                }).ToList();

                return new BillWithDetailsResponseDTO
                {
                    Id = billResponse.Id,
                    ReservationId = billResponse.ReservationId,
                    TotalAmount = billResponse.TotalAmount,
                    CreatedAt = billResponse.CreatedAt,
                    UpdatedAt = billResponse.UpdatedAt,
                    CustomerId = billResponse.CustomerId,
                    ShopId = billResponse.ShopId,
                    BillDetails = billDetailsDTOs 
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<BillResponseDTO> CreateBill(CreateBillRequestDTO createBill)
        {
            try
            {
                var bill = new Bill
                {
                    Id = Guid.NewGuid(),
                    ReservationId = createBill.ReservationId,
                    TotalAmount = createBill.TotalAmount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CustomerId = createBill.CustomerId,
                    ShopId = createBill.ShopId
                };

                var response = await _client.From<Bill>().Insert(bill);
                if (response == null) throw new Exception("Error creating bill.");

                return new BillResponseDTO
                {
                    Id = bill.Id,
                    ReservationId = bill.ReservationId,
                    TotalAmount = bill.TotalAmount,
                    CreatedAt = bill.CreatedAt,
                    UpdatedAt = bill.UpdatedAt,
                    CustomerId = bill.CustomerId,
                    ShopId = bill.ShopId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<BillResponseDTO> UpdateBill(Guid id, UpdateBillRequestDTO updateBill)
        {
            try
            {
                var bill = await _client
                    .From<Bill>()
                    .Where(x => x.Id == id)
                    .Single();

                if (bill == null) throw new Exception("Bill not found.");

                bill.TotalAmount = updateBill.TotalAmount;
                bill.UpdatedAt = DateTime.UtcNow;

                var updatedBill = await _client
                    .From<Bill>()
                    .Where(x => x.Id == id)
                    .Update(bill);

                if (updatedBill == null) throw new Exception("Error updating bill.");

                return new BillResponseDTO
                {
                    Id = bill.Id,
                    ReservationId = bill.ReservationId,
                    TotalAmount = bill.TotalAmount,
                    CreatedAt = bill.CreatedAt,
                    UpdatedAt = bill.UpdatedAt,
                    CustomerId = bill.CustomerId,
                    ShopId = bill.ShopId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<DeleteBillResponseDTO> DeleteBill(Guid id)
        {
            try
            {
                var bill = await GetBillById(id);
                if (bill == null)
                    return new DeleteBillResponseDTO { IsDeleted = false, Message = "Bill not found." };

                await _client
                    .From<Bill>()
                    .Where(x => x.Id == id)
                    .Delete();

                return new DeleteBillResponseDTO
                {
                    IsDeleted = true,
                    Message = "Bill successfully deleted."
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
    }
}

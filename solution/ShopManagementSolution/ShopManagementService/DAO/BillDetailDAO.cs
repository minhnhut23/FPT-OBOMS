using BusinessObject.DTOs.BillDetailDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessObject.Services
{
    public class BillDetailDAO
    {
        private readonly Client _client;

        public BillDetailDAO(Client client)
        {
            _client = client;
        }

        public async Task<List<BillDetailResponseDTO>> GetAllBillDetails()
        {
            try
            {
                var response = await _client.From<BillDetail>().Get();
                var billDetails = response.Models;

                return billDetails.Select(bd => new BillDetailResponseDTO
                {
                    Id = bd.Id,
                    MenuItemId = bd.MenuItemId,
                    Quantity = bd.Quantity,
                    Price = bd.Price,
                    CreatedAt = bd.CreatedAt,
                    UpdatedAt = bd.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<BillDetailResponseDTO?> GetBillDetailById(Guid id)
        {
            try
            {
                var response = await _client.From<BillDetail>().Where(x => x.Id == id).Single();

                if (response == null) return null;

                return new BillDetailResponseDTO
                {
                    Id = response.Id,
                    MenuItemId = response.MenuItemId,
                    Quantity = response.Quantity,
                    Price = response.Price,
                    CreatedAt = response.CreatedAt,
                    UpdatedAt = response.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<BillDetailResponseDTO> CreateBillDetail(CreateBillDetailRequestDTO createBillDetail)
        {
            try
            {
                var billDetail = new BillDetail
                {
                    Id = Guid.NewGuid(),
                    MenuItemId = createBillDetail.MenuItemId,
                    Quantity = createBillDetail.Quantity,
                    Price = createBillDetail.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var response = await _client.From<BillDetail>().Insert(billDetail);
                if (response == null) throw new Exception("Error creating bill detail.");

                return new BillDetailResponseDTO
                {
                    Id = billDetail.Id,
                    MenuItemId = billDetail.MenuItemId,
                    Quantity = billDetail.Quantity,
                    Price = billDetail.Price,
                    CreatedAt = billDetail.CreatedAt,
                    UpdatedAt = billDetail.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }


        public async Task<BillDetailResponseDTO> UpdateBillDetail(Guid id, UpdateBillDetailRequestDTO updateBillDetail)
        {
            try
            {
                var billDetail = await _client.From<BillDetail>().Where(x => x.Id == id).Single();
                if (billDetail == null) throw new Exception("Bill detail not found.");

                billDetail.Quantity = updateBillDetail.Quantity;
                billDetail.Price = updateBillDetail.Price;
                billDetail.UpdatedAt = DateTime.UtcNow;

                var updatedResponse = await _client.From<BillDetail>().Where(x => x.Id == id).Update(billDetail);
                if (updatedResponse == null) throw new Exception("Error updating bill detail.");

                return new BillDetailResponseDTO
                {
                    Id = billDetail.Id,
                    BillId = billDetail.BillId,
                    MenuItemId = billDetail.MenuItemId,
                    Quantity = billDetail.Quantity,
                    Price = billDetail.Price,
                    CreatedAt = billDetail.CreatedAt,
                    UpdatedAt = billDetail.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<DeleteBillDetailResponseDTO> DeleteBillDetail(Guid id)
        {
            try
            {
                var billDetail = await GetBillDetailById(id);
                if (billDetail == null) return new DeleteBillDetailResponseDTO { IsDeleted = false, Message = "Bill detail not found." };

                await _client.From<BillDetail>().Where(x => x.Id == id).Delete();

                return new DeleteBillDetailResponseDTO { IsDeleted = true, Message = "Bill detail successfully deleted." };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
    }
}

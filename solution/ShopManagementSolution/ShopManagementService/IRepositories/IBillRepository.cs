using BusinessObject.DTOs.BillDTO;
using BusinessObject.DTOs.TableDTO;
using Microsoft.AspNetCore.Mvc;

namespace ShopManagementService.IRepositories
{
    public interface IBillRepository
    {
        Task<List<BillResponseDTO>> GetAllBills();
        Task<BillWithDetailsResponseDTO?> GetBillByID(Guid id);
        Task<BillWithDetailsResponseDTO> GetBillByTableID(Guid tableId);

        Task<BillResponseDTO> CreateBill(CreateBillRequestDTO createBill);
        Task<BillResponseDTO> UpdateBill(Guid id, UpdateBillRequestDTO updateBill);
        Task<DeleteBillResponseDTO> DeleteBill(Guid id);
        Task<string> GenerateAndPrintBillPdf(Guid billId);
    }

}

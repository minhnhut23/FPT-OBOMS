
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
        Task<BillResponseStatusDTO> CreateBill(CreateBillRequestDTO createBill);
        Task<BillResponseStatusDTO> UpdateBill(Guid id, UpdateBillRequestDTO updateBill);
        Task<BillResponseStatusDTO> DeleteBill(Guid id);
        Task<string> GenerateAndPrintBillPdf(Guid billId);
    }

}

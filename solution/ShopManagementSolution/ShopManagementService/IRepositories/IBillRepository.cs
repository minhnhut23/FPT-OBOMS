using BusinessObject.DTOs.BillDTO;
using BusinessObject.DTOs.TableDTO;

namespace ShopManagementService.IRepositories
{
    public interface IBillRepository
    {
        Task<List<BillResponseDTO>> GetAllBills();
        Task<BillWithDetailsResponseDTO?> GetBillById(Guid id);
        Task<BillWithDetailsResponseDTO?> GetDraftBillById(Guid id);
        Task<BillResponseDTO> CreateBill(CreateBillRequestDTO createBill);
        Task<BillResponseDTO> UpdateBill(Guid id, UpdateBillRequestDTO updateBill);
        Task<DeleteBillResponseDTO> DeleteBill(Guid id);
        Task<string> GenerateAndPrintBillPdf(Guid billId);
    }

}

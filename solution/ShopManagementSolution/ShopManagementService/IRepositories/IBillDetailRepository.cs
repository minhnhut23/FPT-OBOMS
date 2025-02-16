using BusinessObject.DTOs.BillDetailDTO;

namespace ShopManagementService.IRepositories
{
    public interface IBillDetailRepository
    {
        Task<List<BillDetailResponseDTO>> GetAllBillDetails();
        Task<BillDetailResponseDTO?> GetBillDetailById(Guid id);
        Task<BillDetailResponseDTO> CreateBillDetail(CreateBillDetailRequestDTO createBillDetail);
        Task<BillDetailResponseDTO> UpdateBillDetail(Guid id, UpdateBillDetailRequestDTO updateBillDetail);
        Task<DeleteBillDetailResponseDTO> DeleteBillDetail(Guid id);
    }
}

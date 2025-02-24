using BusinessObject.DTOs.BillDetailDTO;
using BusinessObject.DTOs.BillDTO;

namespace ShopManagementService.IRepositories
{
    public interface IBillDetailRepository
    {
        Task<BillDetailResponseDTO?> GetBillDetailByID(Guid id);
        Task<BillDetailResponseDTO> CreateBillDetail(CreateBillDetailRequestDTO createBillDetail);
        Task<BillDetailResponseDTO> UpdateBillDetail(Guid id, UpdateBillDetailRequestDTO updateBillDetail);
        Task<DeleteBillDetailResponseDTO> DeleteBillDetail(Guid id);
    }
}

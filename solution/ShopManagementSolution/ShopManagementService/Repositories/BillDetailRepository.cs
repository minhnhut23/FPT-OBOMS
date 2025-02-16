using BusinessObject.DTOs.BillDetailDTO;
using BusinessObject.Services;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories
{
    public class BillDetailRepository : IBillDetailRepository
    {
        private readonly BillDetailDAO _billDetailDAO;

        public BillDetailRepository(BillDetailDAO billDetailDAO)
        {
            _billDetailDAO = billDetailDAO;
        }

        public async Task<List<BillDetailResponseDTO>> GetAllBillDetails()
        {
            return await _billDetailDAO.GetAllBillDetails();
        }

        public async Task<BillDetailResponseDTO?> GetBillDetailById(Guid id)
        {
            return await _billDetailDAO.GetBillDetailById(id);
        }

        public async Task<BillDetailResponseDTO> CreateBillDetail(CreateBillDetailRequestDTO createBillDetail)
        {
            return await _billDetailDAO.CreateBillDetail(createBillDetail);
        }

        public async Task<BillDetailResponseDTO> UpdateBillDetail(Guid id, UpdateBillDetailRequestDTO updateBillDetail)
        {
            return await _billDetailDAO.UpdateBillDetail(id, updateBillDetail);
        }

        public async Task<DeleteBillDetailResponseDTO> DeleteBillDetail(Guid id)
        {
            return await _billDetailDAO.DeleteBillDetail(id);
        }
    }
}

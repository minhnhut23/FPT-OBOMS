using BusinessObject.DTOs.BillDetailDTO;
using ShopManagementService.DAO;
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

        public async Task<BillDetailResponseDTO?> GetBillDetailByID(Guid id)
        {
            return await _billDetailDAO.GetBillDetailByID(id);
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

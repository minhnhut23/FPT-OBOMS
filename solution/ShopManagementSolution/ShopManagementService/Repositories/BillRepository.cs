using BusinessObject.DTOs.BillDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Services;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly BillDAO _billDao;

        public BillRepository(BillDAO billDao) => _billDao = billDao;

        public Task<List<BillResponseDTO>> GetAllBills()
            => _billDao.GetAllBills();

        public Task<BillWithDetailsResponseDTO?> GetBillById(Guid id)
            => _billDao.GetBillById(id);

        public Task<BillWithDetailsResponseDTO?> GetDraftBillById(Guid id)
            => _billDao.GetDraftBillById(id);

        public Task<BillResponseDTO> CreateBill(CreateBillRequestDTO createBill)
            => _billDao.CreateBill(createBill);

        public Task<BillResponseDTO> UpdateBill(Guid id, UpdateBillRequestDTO updateBill)
            => _billDao.UpdateBill(id, updateBill);

        public Task<DeleteBillResponseDTO> DeleteBill(Guid id)
            => _billDao.DeleteBill(id);

        public Task<string> GenerateAndPrintBillPdf(Guid billId)
            => _billDao.GenerateAndPrintBillPdf(billId);

        public Task<Guid> GetBillIdByTableId(Guid tableId)
             => _billDao.GetBillIdByTableId(tableId);
    }
}

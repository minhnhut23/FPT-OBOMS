using BusinessObject.DTOs.BillDTO;
using ShopManagementService.DAO;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly BillDAO _billDao;

        public BillRepository(BillDAO billDao) => _billDao = billDao;

        public Task<List<BillResponseDTO>> GetAllBills()
            => _billDao.GetAllBills();

        public Task<BillWithDetailsResponseDTO?> GetBillByID(Guid id)
            => _billDao.GetBillByID(id);

        public Task<BillResponseStatusDTO> CreateBill(CreateBillRequestDTO createBill)
            => _billDao.CreateBill(createBill);

        public Task<BillResponseStatusDTO> UpdateBill(Guid id, UpdateBillRequestDTO updateBill)
            => _billDao.UpdateBill(id, updateBill);

        public Task<BillResponseStatusDTO> DeleteBill(Guid id)
            => _billDao.DeleteBill(id);

        public Task<string> GenerateAndPrintBillPdf(Guid billId)
            => _billDao.GenerateAndPrintBillPdf(billId);

        public Task<BillWithDetailsResponseDTO> GetBillByTableID(Guid tableId)
            => _billDao.GetBillByTableID(tableId);
        
    }
}

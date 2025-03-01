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

        public Task<BillWithDetailsResponseDTO?> GetBillByID(Guid id)
            => _billDao.GetBillByID(id);

        public Task<BillResponseDTO> CreateBill(CreateBillRequestDTO createBill)
            => _billDao.CreateBill(createBill);

        public Task<BillResponseDTO> UpdateBill(Guid id, UpdateBillRequestDTO updateBill)
            => _billDao.UpdateBill(id, updateBill);

        public Task<DeleteBillResponseDTO> DeleteBill(Guid id)
            => _billDao.DeleteBill(id);

        public Task<string> GenerateAndPrintBillPdf(Guid billId)
            => _billDao.GenerateAndPrintBillPdf(billId);

        public Task<BillWithDetailsResponseDTO> GetBillByTableID(Guid tableId)
             => _billDao.GetBillByTableID(tableId);

       
    }
}

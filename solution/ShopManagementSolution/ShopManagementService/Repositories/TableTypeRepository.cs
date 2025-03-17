using BusinessObject.DTOs.TableTypeDTO;
using ShopManagementService.DAO;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories
{
    public class TableTypeRepository : ITableTypeRepository
    {
        private readonly TableTypeDAO _tableTypeDao;

        public TableTypeRepository(TableTypeDAO tableTypeDao)
        {
            _tableTypeDao = tableTypeDao;
        }

        public async Task<List<GetTableTypeResponseDTO>> GetAllTableTypes()
        {
            return await _tableTypeDao.GetAllTableTypes();
        }

        public Task<GetTableTypeResponseDTO?> GetTableTypeById(Guid id)
            => _tableTypeDao.GetTableTypeById(id);

      
        public Task<GetTableTypeResponseDTO> CreateTableType(AddEditTypeRequestDTO createTableType)
            => _tableTypeDao.CreateTableType(createTableType);

        public Task<GetTableTypeResponseDTO> UpdateTableType(Guid id, AddEditTypeRequestDTO updateTableType)
            => _tableTypeDao.UpdateTableType(id, updateTableType);

        public Task<DeleteTableTypeDTO> DeleteTableType(Guid id)
            => _tableTypeDao.DeleteTableType(id);
    }
}

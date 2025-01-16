using AutoMapper;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Services;
using ShopManagementService.Interfaces.IRepositories;

namespace ShopManagementService.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly TableDAO _tableDao;

        public TableRepository(TableDAO tableDao) => _tableDao = tableDao;

        public Task<(List<GetTableResponseDTO> Tables, PaginationMetadataDTO PaginationMetadata)> GetAllTables(GetTableRequestDTO request)
            => _tableDao.GetAllTables(request);

        public Task<GetTableResponseDTO?> GetTableById(Guid id)
            => _tableDao.GetTableById(id);

        public Task<bool> GetTableByNumber(string number, Guid shopId)
            => _tableDao.GetTableByNumber(number, shopId);

        public Task<GetTableResponseDTO> CreateTable(CreateTableRequestDTO createTable)
            => _tableDao.CreateTable(createTable);

        public Task<GetTableResponseDTO> UpdateTable(Guid id, UpdateTableRequestDTO updateTable)
            => _tableDao.UpdateTable(id, updateTable);

        public Task<DeleteTableRequestDTO> DeleteTable(Guid id)
            => _tableDao.DeleteTable(id);
    }
    
}

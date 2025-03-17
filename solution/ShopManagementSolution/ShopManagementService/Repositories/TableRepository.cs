using AutoMapper;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Services;
using ShopManagementService.IRepositories;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ShopManagementService.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly TableDAO _tableDao;

        public TableRepository(TableDAO tableDao) => _tableDao = tableDao;


        
           

        public Task<GetTableResponseDTO?> GetTableById(Guid id)
            => _tableDao.GetTableById(id);

        public Task<bool> GetTableByNumber(string number, Guid shopId)
            => _tableDao.isTableExistInShop(number, shopId);

        public Task<CUDTableResponseDTO> CreateTable(CreateTableRequestDTO createTable)
            => _tableDao.CreateTable(createTable);

        public Task<CUDTableResponseDTO> UpdateTable(Guid id, UpdateTableRequestDTO updateTable)
            => _tableDao.UpdateTable(id, updateTable);

        public Task<CUDTableResponseDTO> DeleteTable(Guid id)
            => _tableDao.DeleteTable(id);

        public Task<UpdateTableStatusResponseDTO> UpdateTableStatus(Guid tableId, bool isFinish)
         => _tableDao.UpdateTableStatus(tableId,isFinish);

        public Task<(List<GetTableResponseDTO>, TablePaginationDTO)> GetAllTables(Guid shopId, GetTablesRequestDTO request)
        
            => _tableDao.GetAllTables(shopId, request);
        
    }
    
}

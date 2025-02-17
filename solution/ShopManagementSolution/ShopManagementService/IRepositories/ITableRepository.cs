using BusinessObject.DTOs.TableDTO;

namespace ShopManagementService.IRepositories
{
    public interface ITableRepository
    {
        Task<(List<GetTableResponseDTO> Tables, PagingTableDTO PaginationMetadata)> GetAllTables(GetTablesRequestDTO request);
        Task<GetTableResponseDTO?> GetTableById(Guid id);
        Task<bool> GetTableByNumber(string number, Guid shopId);
        Task<GetTableResponseDTO> CreateTable(CreateTableRequestDTO createTable);
        Task<GetTableResponseDTO> UpdateTable(Guid id, UpdateTableRequestDTO updateTable);
        Task<DeleteTableRequestDTO> DeleteTable(Guid id);

        Task<UpdateTableStatusResponseDTO> UpdateTableStatus(Guid tableId, bool isFinish);
    }
}

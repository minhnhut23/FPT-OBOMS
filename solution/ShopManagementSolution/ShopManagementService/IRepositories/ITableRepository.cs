using BusinessObject.DTOs.TableDTO;

namespace ShopManagementService.IRepositories
{
    public interface ITableRepository
    {

        Task<(List<GetTableResponseDTO>, TablePaginationDTO)> GetAllTables(Guid shopId, GetTablesRequestDTO request);

        Task<GetTableResponseDTO?> GetTableById(Guid id);
        Task<bool> GetTableByNumber(string number, Guid shopId);
        Task<CUDTableResponseDTO> CreateTable(CreateTableRequestDTO createTable);
        Task<CUDTableResponseDTO> UpdateTable(Guid id, UpdateTableRequestDTO updateTable);
        Task<CUDTableResponseDTO> DeleteTable(Guid id);

        Task<UpdateTableStatusResponseDTO> UpdateTableStatus(Guid tableId, bool isFinish);
    }
}

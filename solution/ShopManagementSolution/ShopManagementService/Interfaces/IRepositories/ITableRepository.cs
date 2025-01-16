using BusinessObject.DTOs.TableDTO;

namespace ShopManagementService.Interfaces.IRepositories
{
    public interface ITableRepository
    {
        Task<(List<GetTableResponseDTO> Tables, PaginationMetadataDTO PaginationMetadata)> GetAllTables(GetTableRequestDTO request);
        Task<GetTableResponseDTO?> GetTableById(Guid id);
        Task<bool> GetTableByNumber(string number, Guid shopId);
        Task<GetTableResponseDTO> CreateTable(CreateTableRequestDTO createTable);
        Task<GetTableResponseDTO> UpdateTable(Guid id, UpdateTableRequestDTO updateTable);
        Task<DeleteTableRequestDTO> DeleteTable(Guid id);
    }
}

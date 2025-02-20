using BusinessObject.DTOs.TableDTO;
using BusinessObject.DTOs.TableTypeDTO;

namespace ShopManagementService.IRepositories
{
    public interface ITableTypeRepository
    {
        Task<List<GetTableTypeResponseDTO>> GetAllTableTypes();
        Task<GetTableTypeResponseDTO?> GetTableTypeById(Guid id);
        Task<GetTableTypeResponseDTO> CreateTableType(AddEditTypeRequestDTO createTableType);
        Task<GetTableTypeResponseDTO> UpdateTableType(Guid id, AddEditTypeRequestDTO updateTableType);
        Task<DeleteTableTypeDTO> DeleteTableType(Guid id);
    }
}

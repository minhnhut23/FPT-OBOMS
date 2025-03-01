using BusinessObject.DTOs.ShopDTO;
using BusinessObject.DTOs.TableDTO;

namespace ShopManagementService.IRepositories;

public interface IShopRepository
{
    Task<(List<ShopResponseDTO> Shops, TablePaginationDTO PaginationMetadata)> GetAllShops(GetShopRequestDTO request);
    Task<ShopResponseDTO?> GetShopById(Guid id);
    Task<ShopResponseDTO> CreateShop(CreateShopRequestDTO createShop, string token);
    Task<ShopResponseDTO> UpdateShop(Guid id, UpdateShopRequestDTO updateShop);
    Task<DeleteShopResponseDTO> DeleteShop(Guid id);
}

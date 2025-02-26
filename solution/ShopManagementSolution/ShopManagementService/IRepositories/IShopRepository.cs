using BusinessObject.DTOs.ShopDTO;

namespace ShopManagementService.IRepositories;

public interface IShopRepository
{
    Task<List<ShopResponseDTO>> GetAllShops();
    Task<ShopResponseDTO?> GetShopById(Guid id);
    Task<ShopResponseDTO> CreateShop(CreateShopRequestDTO createShop);
    Task<ShopResponseDTO> UpdateShop(Guid id, UpdateShopRequestDTO updateShop);
    Task<DeleteShopResponseDTO> DeleteShop(Guid id);
}

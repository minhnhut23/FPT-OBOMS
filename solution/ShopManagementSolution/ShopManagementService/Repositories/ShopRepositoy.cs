using BusinessObject.DTOs.ShopDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Services;
using ShopManagementService.DAO;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories;

public class ShopRepositoy : IShopRepository
{
    private readonly ShopDAO _dao;

    public ShopRepositoy(ShopDAO dao)
    {
        _dao = dao;
    }

    public Task<ShopResponseDTO> CreateShop(CreateShopRequestDTO createShop, string token) => _dao.CreateShop(createShop, token);

    public Task<DeleteShopResponseDTO> DeleteShop(Guid id, string token) => _dao.DeleteShop(id, token);

    public Task<(List<ShopResponseDTO> Shops, TablePaginationDTO PaginationMetadata)> GetAllShops(GetShopRequestDTO request) => _dao.GetAllShops(request);

    public Task<ShopResponseDTO?> GetShopById(Guid id) => _dao.GetShopById(id);

    public Task<(List<ShopResponseDTO> Shops, TablePaginationDTO PaginationMetadata)> GetShopsByCurrentOwner(GetShopRequestDTO request, string token) => _dao.GetShopsByCurrentOwner(request, token);

    public Task<ShopResponseDTO> UpdateShop(Guid id, UpdateShopRequestDTO updateShop, string token) => _dao.UpdateShop(id, updateShop, token);
}

using BusinessObject.DTOs.ShopDTO;
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

    public Task<ShopResponseDTO> CreateShop(CreateShopRequestDTO createShop) => _dao.CreateShop(createShop);

    public Task<DeleteShopResponseDTO> DeleteShop(Guid id) => _dao.DeleteShop(id);

    public Task<List<ShopResponseDTO>> GetAllShops() => _dao.GetAllShops();

    public Task<ShopResponseDTO?> GetShopById(Guid id) => _dao.GetShopById(id);

    public Task<ShopResponseDTO> UpdateShop(Guid id, UpdateShopRequestDTO updateShop) => _dao.UpdateShop(id, updateShop);
}

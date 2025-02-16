using BusinessObject.DTOs.ProductDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;

using ShopManagementService.DAO;
using ShopManagementService.Interface.Repositories;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDAO _dao;

    public ProductRepository(ProductDAO dao)
    {
        _dao = dao;
    }

    public Task CreateProduct(CreateProductRequestDTO request, string token) => _dao.CreateProduct(request, token);

    public Task<MenuItem> GetProductById(Guid id) => _dao.GetProductById(id);

    public Task UpdateProduct(UpdateProductRequestDTO request, Guid id, string token) => _dao.UpdateProduct(request, id, token);

    public Task DeleteProduct(Guid id) => _dao.DeleteProduct(id);
    public Task<(List<GetProductResponseDTO>, ProductPaginationDTO)> GetAllProducts(GetProductRequestDTO request)
       => _dao.GetAllProducts(request);

}


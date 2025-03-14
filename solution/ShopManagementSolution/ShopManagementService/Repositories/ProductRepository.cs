using BusinessObject.DTOs.ProductDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;

using ShopManagementService.DAO;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDAO _dao;

    public ProductRepository(ProductDAO dao)
    {
        _dao = dao;
    }

    public Task<GetProductResponseDTO> CreateProduct(CreateProductRequestDTO request, string token) => _dao.CreateProduct(request, token);

    public Task<Product> GetProductById(Guid id) => _dao.GetProductById(id);

    public Task<GetProductResponseDTO> UpdateProduct(UpdateProductRequestDTO request, Guid id, string token) => _dao.UpdateProduct(request, id, token);

    public Task DeleteProduct(Guid id, string token) => _dao.DeleteProduct(id, token);

    public Task<(List<GetProductResponseDTO>, ProductPaginationDTO)> GetAllProducts(GetProductRequestDTO request)
       => _dao.GetAllProducts(request);

}


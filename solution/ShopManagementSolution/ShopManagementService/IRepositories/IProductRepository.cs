using BusinessObject.DTOs.ProductDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;

namespace ShopManagementService.IRepositories;

public interface IProductRepository
{
    public Task<(List<GetProductResponseDTO>, ProductPaginationDTO)> GetAllProducts(GetProductRequestDTO request);

    public Task<MenuItem> GetProductById(Guid id);

    public Task CreateProduct(CreateProductRequestDTO request, string token);

    public Task UpdateProduct(UpdateProductRequestDTO request, Guid id, string token);

    public Task DeleteProduct(Guid id);

}
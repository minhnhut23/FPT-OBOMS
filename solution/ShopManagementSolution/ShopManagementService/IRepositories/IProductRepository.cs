using BusinessObject.DTOs.ProductDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;

namespace ShopManagementService.IRepositories;

public interface IProductRepository
{
    public Task<(List<GetProductResponseDTO>, ProductPaginationDTO)> GetAllProducts(GetProductRequestDTO request);

    public Task<Product> GetProductById(Guid id);

    public Task<GetProductResponseDTO> CreateProduct(CreateProductRequestDTO request, string token);

    public Task<GetProductResponseDTO> UpdateProduct(UpdateProductRequestDTO request, Guid id, string token);

    public Task DeleteProduct(Guid id, string token);

}
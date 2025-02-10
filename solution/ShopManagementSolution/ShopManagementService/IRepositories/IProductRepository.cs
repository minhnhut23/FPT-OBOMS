using BusinessObject.DTOs.ProductDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;

namespace ShopManagementService.Interface.Repositories;

public interface IProductRepository
{
    public Task<(List<GetProductResponseDTO>, ProductPaginationDTO)> GetAllProducts(GetProductRequestDTO request);

    public Task<MenuItem> GetProductById(Guid id);

    public Task CreateProduct(CreateProductRequestDTO request);

    public Task UpdateProduct(UpdateProductRequestDTO request, Guid id);

    public Task DeleteProduct(Guid id);

}
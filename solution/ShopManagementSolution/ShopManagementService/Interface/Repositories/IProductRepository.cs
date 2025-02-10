using BusinessObject.DTOs.ProductDTO;
using BusinessObject.Models;

namespace ShopManagementService.Interface.Repositories;

public interface IProductRepository
{
    public Task<MenuItem> GetProductById(Guid id);

    public Task CreateProduct(CreateProductRequestDTO request);

    public Task UpdateProduct(UpdateProductRequestDTO request, Guid id);

    public Task DeleteProduct(Guid id);

}

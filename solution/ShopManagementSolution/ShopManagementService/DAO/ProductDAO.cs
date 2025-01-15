using BusinessObject.DTOs.ProductDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Supabase;

namespace ShopManagementService.DAO;

public class ProductDAO
{
    private readonly Client _client;

    public ProductDAO(Client client)
    {
        _client = client;
    }
    public async Task<MenuItem> GetProductById(Guid id)
    {
        try
        {
            var productResponse = await _client.From<MenuItem>().Where(p => p.Id == id).Single();
            if (productResponse == null)
            {
                throw new Exception("Product not found!");
            }
            return productResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task CreateProduct(CreateProductRequestDTO request)
    {
        try
        {
            var shop = await _client.From<Shop>().Where(s => s.Id == request.ShopId).Single();

            if (shop == null)
            {
                throw new Exception("Shop not found!");
            }

            var product = new MenuItem
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description!,
                Category = request.Category!,
                IsAvailable = true,
                Ingredient = request.Ingredient!,
                NutritionalIfo = request.NutritionalIfo!,
                ShopId = shop.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _client.From<MenuItem>().Insert(product);
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task UpdateProduct(UpdateProductRequestDTO request, Guid id)
    {
        try
        {
            var product = await _client.From<MenuItem>().Where(s => s.Id == id).Single();

            if (product == null)
            {
                throw new Exception("Product not found!");
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Description = request.Description!;
            product.Category = request.Category!;
            product.IsAvailable = request.IsAvailable;
            product.Ingredient = request.Ingredient!;
            product.NutritionalIfo = request.NutritionalIfo!;
            product.UpdatedAt = DateTime.Now;

            await product.Update<MenuItem>();
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task DeleteProduct(Guid id)
    {
        try
        {
            var product = await _client.From<MenuItem>().Where(s => s.Id == id).Single();

            if (product == null)
            {
                throw new Exception("Product not found!");
            }

            await _client.From<MenuItem>().Where(s => s.Id == id).Delete();
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }
}

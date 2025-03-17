using AutoMapper;
using BusinessObject.DTOs.ProductDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using ShopManagementService.Utils;
using Supabase;
using System.IdentityModel.Tokens.Jwt;
using static Supabase.Postgrest.Constants;


namespace ShopManagementService.DAO;

public class ProductDAO
{
    private readonly Client _client;
    private readonly IMapper _mapper;

    public ProductDAO(Client client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;

    }
    public async Task<Product> GetProductById(Guid id)
    {
        try
        {
            var productResponse = await _client.From<Product>().Where(p => p.Id == id).Single();
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
    public async Task<(List<GetProductResponseDTO> Products, ProductPaginationDTO PaginationMetadata)> GetAllProducts(GetProductRequestDTO request)
    {
        try
        {
            var query = _client.From<Product>().Select("*");
            query = ApplyFilters(query, request);

            var counting = _client.From<Product>().Select("*");
            counting = ApplyFilters(counting, request);
            var totalRecordsResponse = await counting.Select("id").Get();
            var totalRecords = totalRecordsResponse.Models?.Count ?? 0;
            var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

            var skip = (request.PageNumber - 1) * request.PageSize;
            var paginatedQuery = query.Range(skip, skip + request.PageSize - 1);

            var productsResponse = await paginatedQuery.Get();
            var products = productsResponse.Models
                .Select(product => _mapper.Map<GetProductResponseDTO>(product))
                .ToList();

            if (totalRecords == 0 || request.PageNumber > totalPages)
            {
                return (
                    new List<GetProductResponseDTO>(),
                    new ProductPaginationDTO
                    {
                        TotalResults = totalRecords,
                        TotalPages = totalPages,
                        CurrentPage = request.PageNumber,
                        PageSize = request.PageSize
                    }
                );
            }

            var paginationMetadata = new ProductPaginationDTO
            {
                TotalResults = totalRecords,
                TotalPages = totalPages,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize
            };

            return (products, paginationMetadata);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching products: {ex.Message}");
        }
    }

    private dynamic ApplyFilters(dynamic query, GetProductRequestDTO request)
    {
        if (!string.IsNullOrEmpty(request.Name))
            query = query.Filter("name", Operator.ILike, $"%{request.Name}%");

        if (request.MinPrice.HasValue)
            query = query.Filter("price", Operator.GreaterThanOrEqual, (float)request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Filter("price", Operator.LessThanOrEqual, (float)request.MaxPrice.Value);

        if (!string.IsNullOrEmpty(request.Category))
            query = query.Filter("category", Operator.ILike, $"%{request.Category}%");

        if (request.IsAvailable.HasValue)
            query = query.Filter("is_available", Operator.Equals, request.IsAvailable.Value);

        return query;
    }

    public async Task<GetProductResponseDTO> CreateProduct(CreateProductRequestDTO request, string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            var accountId = Guid.Parse(claims["sub"]);

            var profile = await _client
                .From<BusinessObject.Models.Profile>()
                .Where(x => x.AccountId == accountId)
                .Single();

            var shop = await _client.From<Shop>().Where(s => s.Id == request.ShopId).Single();

            if (shop == null)
            {
                throw new Exception("Shop not found!");
            }

            if (shop.OwnerId != profile!.Id)
            {
                throw new Exception("You are not shop's owner!");
            }

            string NewImage = "";

            if (request.ProductImage != null)
            {
                if (!ImageValidator.IsValidImage(request.ProductImage, out string error))
                {
                    throw new Exception(error);
                }

                var file = request.ProductImage;
                var fileName = $"{Guid.NewGuid()}_{file.FileName.Trim()}";

                byte[] fileBytes;
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    fileBytes = stream.ToArray();
                }

                await _client.Storage
                    .From("products")
                    .Upload(fileBytes, fileName, new Supabase.Storage.FileOptions { CacheControl = "3600", Upsert = false });

                NewImage = _client.Storage.From("products").GetPublicUrl(fileName);
            }

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description!,
                Category = request.Category!,
                IsAvailable = true,
                Ingredient = request.Ingredient!,
                NutritionalIfo = request.NutritionalIfo!,
                ShopId = shop.Id,
                Image = NewImage,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _client.From<Product>().Insert(product);

            return new GetProductResponseDTO
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category,
                IsAvailable = true,
                Ingredient = product.Ingredient,
                NutritionalInfo = product.NutritionalIfo,
                Image = product.Image,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task<GetProductResponseDTO> UpdateProduct(UpdateProductRequestDTO request, Guid id, string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            var accountId = Guid.Parse(claims["sub"]);

            var profile = await _client
                .From<BusinessObject.Models.Profile>()
                .Where(x => x.AccountId == accountId)
                .Single();

            var product = await _client.From<Product>().Where(s => s.Id == id).Single();

            var shop = await _client.From<Shop>().Where(s => s.Id == product!.ShopId).Single();

            if (shop == null)
            {
                throw new Exception("Shop not found!");
            }

            if (shop.OwnerId != profile!.Id)
            {
                throw new Exception("You are not shop's owner!");
            }

            if (product == null)
            {
                throw new Exception("Product not found!");
            }

            if (request.ProductImage != null)
            {
                if (!ImageValidator.IsValidImage(request.ProductImage, out string error))
                {
                    throw new Exception(error);
                }

                var file = request.ProductImage;
                var fileName = $"{Guid.NewGuid()}_{file.FileName.Trim()}";

                if (!string.IsNullOrEmpty(product.Image))
                {
                    var oldFileName = Path.GetFileName(new Uri(product.Image).AbsolutePath);
                    var deleteResult = await _client.Storage.From("products").Remove(new List<string> { oldFileName });
                }

                byte[] fileBytes;
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    fileBytes = stream.ToArray();
                }

                await _client.Storage
                    .From("products")
                    .Upload(fileBytes, fileName, new Supabase.Storage.FileOptions { CacheControl = "3600", Upsert = false });

                product.Image = _client.Storage.From("products").GetPublicUrl(fileName);
            }

            product.Name = request.Name ?? product.Name;
            product.Price = request.Price ?? product.Price;
            product.Description = request.Description! ?? product.Description;
            product.Category = request.Category! ?? product.Category;
            product.IsAvailable = request.IsAvailable ?? product.IsAvailable;
            product.Ingredient = request.Ingredient! ?? product.Ingredient;
            product.NutritionalIfo = request.NutritionalIfo! ?? product.NutritionalIfo;
            product.UpdatedAt = DateTime.Now;

            await product.Update<Product>();

            return new GetProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category,
                IsAvailable = product.IsAvailable,
                Ingredient = product.Ingredient,
                NutritionalInfo = product.NutritionalIfo,
                Image = product.Image,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task DeleteProduct(Guid id, string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            var accountId = Guid.Parse(claims["sub"]);

            var profile = await _client
                .From<BusinessObject.Models.Profile>()
                .Where(x => x.AccountId == accountId)
                .Single();

            var product = await _client.From<Product>().Where(s => s.Id == id).Single();

            var shop = await _client.From<Shop>().Where(s => s.Id == product!.ShopId).Single();

            if (shop == null)
            {
                throw new Exception("Shop not found!");
            }

            if (shop.OwnerId != profile!.Id)
            {
                throw new Exception("You are not shop's owner!");
            }

            if (product == null)
            {
                throw new Exception("Product not found!");
            }

            await _client.From<Product>().Where(s => s.Id == id).Delete();
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }
}

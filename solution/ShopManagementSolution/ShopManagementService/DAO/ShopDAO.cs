using BusinessObject.DTOs.ShopDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using ShopManagementService.Utils;

namespace BusinessObject.Services
{
    public class ShopDAO
    {
        private readonly Supabase.Client _client;

        public ShopDAO(Supabase.Client client)
        {
            _client = client;
        }

        public async Task<List<ShopResponseDTO>> GetAllShops()
        {
            try
            {
                var shopResponse = await _client.From<Shop>().Get();
                var shops = shopResponse.Models;

                return shops.Select(shop => new ShopResponseDTO
                {
                    Id = shop.Id,
                    Name = shop.Name,
                    Description = shop.Description,
                    Address = shop.Address,
                    PhoneNumber = shop.PhoneNumber,
                    OpeningHours = shop.OpeningHours,
                    ClosingHours = shop.ClosingHours,
                    Rating = shop.Rating,
                    Latitude = shop.Latitude,
                    Longitude = shop.Longitude,
                    OwnerId = shop.OwnerId,
                    Status = ShopStatusHelper.GetShopStatus(shop.OpeningHours, shop.ClosingHours)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<ShopResponseDTO?> GetShopById(Guid id)
        {
            try
            {
                var shopResponse = await _client.From<Shop>().Where(s => s.Id == id).Single();
                if (shopResponse == null) return null;

                TimeSpan now = DateTime.Now.TimeOfDay;

                return new ShopResponseDTO
                {
                    Id = shopResponse.Id,
                    Name = shopResponse.Name,
                    Description = shopResponse.Description,
                    Address = shopResponse.Address,
                    PhoneNumber = shopResponse.PhoneNumber,
                    OpeningHours = shopResponse.OpeningHours,
                    ClosingHours = shopResponse.ClosingHours,
                    Rating = shopResponse.Rating,
                    Latitude = shopResponse.Latitude,
                    Longitude = shopResponse.Longitude,
                    OwnerId = shopResponse.OwnerId,
                    Status = ShopStatusHelper.GetShopStatus(shopResponse.OpeningHours, shopResponse.ClosingHours)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<ShopResponseDTO> CreateShop(CreateShopRequestDTO createShop)
        {
            try
            {
                var shop = new Shop
                {
                    Id = Guid.NewGuid(),
                    Name = createShop.Name,
                    Description = createShop.Description,
                    Address = createShop.Address,
                    PhoneNumber = createShop.PhoneNumber,
                    OpeningHours = createShop.OpeningHours,
                    ClosingHours = createShop.ClosingHours,
                    Rating = createShop.Rating,
                    Latitude = createShop.Latitude,
                    Longitude = createShop.Longitude,
                    OwnerId = createShop.OwnerId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var response = await _client.From<Shop>().Insert(shop);
                if (response == null) throw new Exception("Error creating shop.");

                return new ShopResponseDTO
                {
                    Id = shop.Id,
                    Name = shop.Name,
                    Description = shop.Description,
                    Address = shop.Address,
                    PhoneNumber = shop.PhoneNumber,
                    OpeningHours = shop.OpeningHours,
                    ClosingHours = shop.ClosingHours,
                    Rating = shop.Rating,
                    Latitude = shop.Latitude,
                    Longitude = shop.Longitude,
                    OwnerId = shop.OwnerId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<ShopResponseDTO> UpdateShop(Guid id, UpdateShopRequestDTO updateShop)
        {
            try
            {
                var shop = await _client.From<Shop>().Where(s => s.Id == id).Single();
                if (shop == null) throw new Exception("Shop not found.");

                shop.Name = updateShop.Name ?? shop.Name;
                shop.Description = updateShop.Description ?? shop.Description;
                shop.Address = updateShop.Address ?? shop.Address;
                shop.PhoneNumber = updateShop.PhoneNumber ?? shop.PhoneNumber;
                shop.OpeningHours = updateShop.OpeningHours ?? shop.OpeningHours;
                shop.ClosingHours = updateShop.ClosingHours ?? shop.ClosingHours;
                shop.Rating = updateShop.Rating ?? shop.Rating;
                shop.Latitude = updateShop.Latitude ?? shop.Latitude;
                shop.Longitude = updateShop.Longitude ?? shop.Longitude;
                shop.UpdatedAt = DateTime.UtcNow;

                var response = await _client.From<Shop>().Update(shop);
                if (response == null) throw new Exception("Error updating shop.");

                return new ShopResponseDTO
                {
                    Id = shop.Id,
                    Name = shop.Name,
                    Description = shop.Description,
                    Address = shop.Address,
                    PhoneNumber = shop.PhoneNumber,
                    OpeningHours = shop.OpeningHours,
                    ClosingHours = shop.ClosingHours,
                    Rating = shop.Rating,
                    Latitude = shop.Latitude,
                    Longitude = shop.Longitude,
                    OwnerId = shop.OwnerId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<DeleteShopResponseDTO> DeleteShop(Guid id)
        {
            try
            {
                var shop = await GetShopById(id);
                if (shop == null)
                    return new DeleteShopResponseDTO { IsDeleted = false, Message = "Shop not found." };

                await _client.From<Shop>().Where(s => s.Id == id).Delete();

                return new DeleteShopResponseDTO
                {
                    IsDeleted = true,
                    Message = "Shop successfully deleted."
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
    }
}

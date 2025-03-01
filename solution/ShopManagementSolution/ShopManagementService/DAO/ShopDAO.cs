using BusinessObject.DTOs.ShopDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using ShopManagementService.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessObject.Services
{
    public class ShopDAO
    {
        private readonly Supabase.Client _client;

        public ShopDAO(Supabase.Client client)
        {
            _client = client;
        }
        
        public async Task<(List<ShopResponseDTO> Shops, TablePaginationDTO PaginationMetadata)> GetAllShops(GetShopRequestDTO request)
        {
            try
            {
                var query = _client.From<Shop>().Select("*");

                var totalRecordsResponse = await _client.From<Shop>().Select("id").Get();
                var totalRecords = totalRecordsResponse.Models?.Count ?? 0;
                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

                var skip = (request.PageNumber - 1) * request.PageSize;
                var paginatedQuery = query.Range(skip, skip + request.PageSize - 1);

                var shopResponse = await paginatedQuery.Get();
                var shops = shopResponse.Models.Select(shop => new ShopResponseDTO
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

                if (totalRecords == 0 || request.PageNumber > totalPages)
                {
                    return (
                        new List<ShopResponseDTO>(),
                        new TablePaginationDTO
                        {
                            TotalResults = totalRecords,
                            TotalPages = totalPages,
                            CurrentPage = request.PageNumber,
                            PageSize = request.PageSize
                        }
                    );
                }

                var paginationMetadata = new TablePaginationDTO
                {
                    TotalResults = totalRecords,
                    TotalPages = totalPages,
                    CurrentPage = request.PageNumber,
                    PageSize = request.PageSize
                };

                return (shops, paginationMetadata);
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

        public async Task<ShopResponseDTO> CreateShop(CreateShopRequestDTO createShop, string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
                var accountId = Guid.Parse(claims["sub"]);

                var profile = await _client
                    .From<Profile>()
                    .Where(x => x.AccountId == accountId)
                    .Single();

                if (profile!.Role != "Owner")
                {
                    throw new Exception("You are not an owner!");
                }

                var shop = new Shop
                {
                    Name = createShop.Name,
                    Description = createShop.Description,
                    Address = createShop.Address,
                    PhoneNumber = createShop.PhoneNumber,
                    OpeningHours = TimeSpan.Parse(createShop.OpeningHours),
                    ClosingHours = TimeSpan.Parse(createShop.ClosingHours),
                    Latitude = createShop.Latitude,
                    Longitude = createShop.Longitude,
                    OwnerId = profile.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                try
                {
                    var response = await _client
                        .From<Shop>()
                        .Insert(shop);                  

                    var insertedShops = JsonConvert.DeserializeObject<List<Shop>>(response.Content.ToString());

                    if (insertedShops == null || insertedShops.Count == 0)
                        throw new Exception("Error inserting shop into Supabase.");

                    shop.Id = insertedShops[0].Id; 
                }
                catch (Exception ex)
                {
                    throw;
                }

                var subs = await _client
                    .From<Subscriptions>()
                    .Where(x => x.Id == createShop.SubscriptionId)
                    .Single();

                if (subs == null)
                {
                    throw new Exception("Subscription not found.");
                }

                if (shop == null || shop.Id == Guid.Empty)
                {
                    throw new Exception("Shop creation failed or ShopId is missing.");
                }

                var subShop = new ShopSubscriptions
                {
                    SubscriptionId = subs.Id,
                    ShopId = shop.Id, 
                    StartedAt = DateTime.UtcNow,
                    EndedAt = DateTime.UtcNow.AddMonths(subs.NumberOfMonths),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                Console.WriteLine($"Creating ShopSubscription: ShopId={subShop.ShopId}, SubscriptionId={subShop.SubscriptionId}");

                var res = await _client
                    .From<ShopSubscriptions>()
                    .Insert(subShop);

                var insertedSubShop = res.Content?.FirstOrDefault();
                if (insertedSubShop == null)
                {
                    throw new Exception("Failed to insert ShopSubscription.");
                }

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
                    OwnerId = shop.OwnerId,
                    SubscriptionName = subs.Name,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<ShopResponseDTO> UpdateShop(Guid id, UpdateShopRequestDTO updateShop, string token)
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

                var shop = await _client.From<Shop>().Where(s => s.Id == id).Single();

                if (shop == null)
                {
                    throw new Exception("Shop not found!");
                }

                if (shop.OwnerId != profile!.Id)
                {
                    throw new Exception("You are not shop's owner!");
                }

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

using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Newtonsoft.Json;
using Supabase;
using Supabase.Postgrest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessObject.Services
{
    public class TableDAO
    {
        private readonly Supabase.Client _client;

        public TableDAO(Supabase.Client client)
        {
            _client = client;
        }

        public async Task<List<GetTableResponseDTO>> GetAllTables()
        {
            var response = await _client
                .From<Table>()
                .Get();
            var tables = response.Models;

         
            var listTables = tables.
                Select(table => new GetTableResponseDTO
                    {
                        Id = table.Id,
                        TableNumber = table.TableNumber,
                        Capacity = table.Capacity,
                        Status = table.Status,
                        LocationDescription = table.LocationDescription,
                        CreatedAt = table.CreatedAt,
                        UpdatedAt = table.UpdatedAt
                    })
                .ToList();

            return listTables;
        }

        public async Task<GetTableResponseDTO?> GetTableById(Guid id)
        {
            try
            {
                var response = await _client
                    .From<Table>()
                     .Where(x => x.Id == id)
                    .Single();
                if (response == null)
                {
                    throw new Exception("Table Not Found!");
                }
                else
                {
                    var tableDetail = new GetTableResponseDTO
                    {
                        Id = response.Id,
                        TableNumber = response.TableNumber,
                        Capacity = response.Capacity,
                        Status = response.Status,
                        LocationDescription = response.LocationDescription,
                        CreatedAt = response.CreatedAt,
                        UpdatedAt = response.UpdatedAt
                    };
                    return tableDetail;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
        public async Task<bool> GetTableByNumber(string number, Guid shopId)
        {
            if (!await IsShopExists(shopId))
            {
                throw new Exception("Error! Shop ID does not exist!");
            }

            try
            {
                var response = await _client
                    .From<Table>()
                    .Where(x => x.TableNumber == number && x.ShopId == shopId)
                    .Single();
                return response == null;
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<bool> IsShopExists(Guid shopId)
        {
            try
            {
                var response = await _client
                    .From<Shop>() 
                    .Where(x => x.Id == shopId)
                    .Single();

                return response != null; 
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<GetTableResponseDTO> CreateTable(CreateTableRequestDTO createTable)
        {
            if (!await IsShopExists(createTable.ShopId))
            {
                throw new Exception("Error! Shop ID does not exist!");
            }

            if (!await GetTableByNumber(createTable.TableNumber, createTable.ShopId))
            {
                throw new Exception("Error! Table Number already exists!");
            }
            if (createTable.Capacity <= 0)
            {
                throw new Exception("Capacity must be a positive number.");
            }
            var table = new Table
            {
                Id = Guid.NewGuid(),
                TableNumber = createTable.TableNumber,
                Capacity = createTable.Capacity,
                Status = createTable.Status,
                LocationDescription = createTable.LocationDescription,
                ShopId = createTable.ShopId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var response = await _client
                .From<Table>()
                .Insert(table);
            if (response == null)
            {
                throw new Exception("Error! Insert error!");
            }

            var tableResponse = new GetTableResponseDTO
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity,
                Status = table.Status,
                LocationDescription = table.LocationDescription,
                CreatedAt = table.CreatedAt
            };

            return tableResponse;
        }


        public async Task<GetTableResponseDTO> UpdateTable(Guid id, UpdateTableRequestDTO updateTable)
        {
            try
            {
                var response = await _client
                    .From<Table>()
                    .Where(x => x.Id == id)
                    .Single();
                if (response == null)
                {
                    throw new Exception("Table not found!");
                }
                response.TableNumber = updateTable.TableNumber;
                response.Capacity = updateTable.Capacity;
                response.Status = updateTable.Status;
                response.LocationDescription = updateTable.LocationDescription;
                response.UpdatedAt = DateTime.UtcNow;

                var updateResponse = await _client
                    .From<Table>()
                    .Where(x => x.Id == id)
                    .Update(response);
                if (updateResponse == null)
                {
                    throw new Exception("Error! Update error!");
                }
                var updatedTableDTO = new GetTableResponseDTO
                {
                    Id = response.Id,
                    TableNumber = response.TableNumber,
                    Capacity = response.Capacity,
                    Status = response.Status,
                    LocationDescription = response.LocationDescription,
                    CreatedAt = response.CreatedAt,
                    UpdatedAt = response.UpdatedAt
                };

                return updatedTableDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<DeleteTableRequestDTO> DeleteTable(Guid id)
        {
            try
            {
                if (GetTableById(id)!=null)
                {
                    return new DeleteTableRequestDTO
                    {
                        IsDeleted = false,
                        Message = "Table not found"
                    };
                }
                await _client
                     .From<Table>()
                     .Where(x => x.Id == id)
                     .Delete();
                
                return new DeleteTableRequestDTO
                {
                    IsDeleted = true,
                    Message = "Table successfully deleted."
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }


    }
}

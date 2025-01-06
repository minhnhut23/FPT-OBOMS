
using BusinessObject.DTOs.TableTypeDTO;
using BusinessObject.Models;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessObject.Services
{
    public class TableTypeDAO
    {
        private readonly Supabase.Client _client;

        public TableTypeDAO(Supabase.Client client)
        {
            _client = client;
        }

        public async Task<List<GetTableTypeResponseDTO>> GetAllTableTypes()
        {
            try
            {
                var response = await _client
                    .From<TableType>()
                    .Get();

                var tableTypes = response.Models
                    .Select(type => new GetTableTypeResponseDTO
                    {
                        Id = type.Id,
                        Name = type.Name,
                        Description = type.Description,
                        CreatedAt = type.CreatedAt,
                        UpdatedAt = type.UpdatedAt
                    })
                    .ToList();

                return tableTypes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching Table Types: {ex.Message}");
            }
        }

        // Get Table Type by ID
        public async Task<GetTableTypeResponseDTO?> GetTableTypeById(Guid id)
        {
            try
            {
                var response = await _client
                    .From<TableType>()
                    .Where(x => x.Id == id)
                    .Single();

                if (response == null)
                {
                    throw new Exception("Table Type not found!");
                }

                return new GetTableTypeResponseDTO
                {
                    Id = response.Id,
                    Name = response.Name,
                    Description = response.Description,
                    CreatedAt = response.CreatedAt,
                    UpdatedAt = response.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching Table Type by ID: {ex.Message}");
            }
        }

        // Create a new Table Type
        public async Task<GetTableTypeResponseDTO> CreateTableType(AddEditTypeRequestDTO createTableType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createTableType.Name))
                {
                    throw new Exception("Name is required!");
                }

                var tableType = new TableType
                {
                    Id = Guid.NewGuid(),
                    Name = createTableType.Name,
                    Description = createTableType.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                var response = await _client
                    .From<TableType>()
                    .Insert(tableType);

                var createdType = response.Models.FirstOrDefault();

                if (createdType == null)
                {
                    throw new Exception("Failed to create Table Type.");
                }

                return new GetTableTypeResponseDTO
                {
                    Id = createdType.Id,
                    Name = createdType.Name,
                    Description = createdType.Description,
                    CreatedAt = createdType.CreatedAt,
                    UpdatedAt = createdType.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating Table Type: {ex.Message}");
            }
        }

        // Update Table Type
        public async Task<GetTableTypeResponseDTO> UpdateTableType(Guid id, AddEditTypeRequestDTO updateTableType)
        {
            try
            {
                var response = await _client
                    .From<TableType>()
                    .Where(x => x.Id == id)
                    .Single();

                if (response == null)
                {
                    throw new Exception("Table Type not found!");
                }

                response.Name = updateTableType.Name;
                response.Description = updateTableType.Description;
                response.UpdatedAt = DateTime.UtcNow;

                var updatedResponse = await _client
                    .From<TableType>()
                    .Where(x => x.Id == id)
                    .Update(response);

                var updatedType = updatedResponse.Models.FirstOrDefault();

                if (updatedType == null)
                {
                    throw new Exception("Failed to update Table Type.");
                }

                return new GetTableTypeResponseDTO
                {
                    Id = updatedType.Id,
                    Name = updatedType.Name,
                    Description = updatedType.Description,
                    CreatedAt = updatedType.CreatedAt,
                    UpdatedAt = updatedType.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating Table Type: {ex.Message}");
            }
        }

        // Delete Table Type
        public async Task<DeleteTableTypeDTO> DeleteTableType(Guid id)
        {
            try
            {
                var existingType = await GetTableTypeById(id);

                if (existingType == null)
                {
                    return new DeleteTableTypeDTO
                    {
                        IsDeleted = false,
                        Message = "Table Type not found."
                    };
                }

                await _client
                    .From<TableType>()
                    .Where(x => x.Id == id)
                    .Delete();

                return new DeleteTableTypeDTO
                {
                    IsDeleted = true,
                    Message = "Table Type successfully deleted."
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting Table Type: {ex.Message}");
            }
        }
    }
}

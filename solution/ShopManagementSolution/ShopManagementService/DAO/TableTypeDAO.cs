using BusinessObject.DTOs.TableTypeDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Supabase;

namespace ShopManagementService.DAO
{
    public class TableTypeDAO
    {
        private readonly Client _client;

        public TableTypeDAO(Client client)
        {
            _client = client;
        }

        public async Task<List<GetTableTypeResponseDTO>> GetAllTableTypes()
        {
            try
            {
                var response = await _client.From<TableType>().Get();
                return response.Models.Select(type => new GetTableTypeResponseDTO
                {
                    Id = type.Id,
                    Name = type.Name,
                    Description = type.Description,
                    PriceByHour = type.PriceByHour,
                    CreatedAt = type.CreatedAt,
                    UpdatedAt = type.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<GetTableTypeResponseDTO?> GetTableTypeById(Guid id)
        {
            try
            {
                var response = await _client.From<TableType>().Where(x => x.Id == id).Single();
                if (response == null)
                    throw new Exception("Table Type not found!");

                return new GetTableTypeResponseDTO
                {
                    Id = response.Id,
                    Name = response.Name,
                    Description = response.Description,
                    PriceByHour = response.PriceByHour,
                    CreatedAt = response.CreatedAt,
                    UpdatedAt = response.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<GetTableTypeResponseDTO> CreateTableType(AddEditTypeRequestDTO createTableType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createTableType.Name))
                    throw new Exception("Name is required!");

                var tableType = new TableType
                {
                    Id = Guid.NewGuid(),
                    Name = createTableType.Name,
                    Description = createTableType.Description,
                    PriceByHour = createTableType.PriceByHour,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                var response = await _client.From<TableType>().Insert(tableType);
                var createdType = response.Models.FirstOrDefault();
                if (createdType == null)
                    throw new Exception("Failed to create Table Type.");

                return new GetTableTypeResponseDTO
                {
                    Id = createdType.Id,
                    Name = createdType.Name,
                    Description = createdType.Description,
                    PriceByHour = createdType.PriceByHour,
                    CreatedAt = createdType.CreatedAt,
                    UpdatedAt = createdType.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<GetTableTypeResponseDTO> UpdateTableType(Guid id, AddEditTypeRequestDTO updateTableType)
        {
            try
            {
                var response = await _client.From<TableType>().Where(x => x.Id == id).Single();
                if (response == null)
                    throw new Exception("Table Type not found!");

                response.Name = updateTableType.Name;
                response.Description = updateTableType.Description;
                response.PriceByHour = updateTableType.PriceByHour;
                response.UpdatedAt = DateTime.UtcNow;

                var updatedResponse = await _client.From<TableType>().Where(x => x.Id == id).Update(response);
                var updatedType = updatedResponse.Models.FirstOrDefault();
                if (updatedType == null)
                    throw new Exception("Failed to update Table Type.");

                return new GetTableTypeResponseDTO
                {
                    Id = updatedType.Id,
                    Name = updatedType.Name,
                    Description = updatedType.Description,
                    PriceByHour = updatedType.PriceByHour,
                    CreatedAt = updatedType.CreatedAt,
                    UpdatedAt = updatedType.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<DeleteTableTypeDTO> DeleteTableType(Guid id)
        {
            try
            {
                var count = await _client.From<Table>().Where(x => x.TypeId == id).Get();
                if (count != null)
                {
                    return new DeleteTableTypeDTO
                    {
                        IsDeleted = false,
                        Message = "Cannot delete TableType. Some tables are using this type."
                    };
                }
                var existingType = await GetTableTypeById(id);
                if (existingType == null)
                {
                    return new DeleteTableTypeDTO
                    {
                        IsDeleted = false,
                        Message = "Table Type not found."
                    };
                }

                await _client.From<TableType>().Where(x => x.Id == id).Delete();
                return new DeleteTableTypeDTO
                {
                    IsDeleted = true,
                    Message = "Table Type successfully deleted."
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
    }
}

using AutoMapper;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using static Supabase.Postgrest.Constants;

namespace BusinessObject.Services
{
    public class TableDAO
    {
        private readonly Supabase.Client _client;
        private readonly IMapper _mapper;

        public TableDAO(Supabase.Client client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<(List<GetTableResponseDTO> Tables, DTOs.TableDTO.PaginationMetadataDTO PaginationMetadata)> GetAllTables(GetTableRequestDTO request)
        {
            try
            {
                var query = _client.From<Table>().Select("*");
                query = ApplyFilters(query, request);

                var counting = _client.From<Table>().Select("*");
                counting = ApplyFilters(counting, request);
                var totalRecordsResponse = await counting.Select("id").Get();
                var totalRecords = totalRecordsResponse.Models?.Count ?? 0;
                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);


                var skip = (request.PageNumber - 1) * request.PageSize;
                var paginatedQuery = query.Range(skip, skip + request.PageSize - 1);

                var tablesResponse = await paginatedQuery.Get();
                var tableTypesResponse = await _client.From<TableType>().Select("*").Get();
                var typeNameDict = tableTypesResponse.Models.ToDictionary(tt => tt.Id, tt => tt.Name);
                var tables = tablesResponse.Models
                    .Select(table =>
                    {
                        var dto = _mapper.Map<GetTableResponseDTO>(table);
                        dto.TableType = typeNameDict.ContainsKey(table.TypeId) ? typeNameDict[table.TypeId] : "Unknown";
                        return dto;
                    })
                    .ToList();

                if (totalRecords == 0 || request.PageNumber > totalPages)
                {
                    return (
                        new List<GetTableResponseDTO>(),
                        new DTOs.TableDTO.PaginationMetadataDTO
                        {
                            TotalResults = totalRecords,
                            TotalPages = totalPages,
                            CurrentPage = request.PageNumber,
                            PageSize = request.PageSize
                        }
                    );
                }

                var paginationMetadata = new DTOs.TableDTO.PaginationMetadataDTO
                {
                    TotalResults = totalRecords,
                    TotalPages = totalPages,
                    CurrentPage = request.PageNumber,
                    PageSize = request.PageSize
                };

                return (tables, paginationMetadata);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching tables: {ex.Message}");
            }
        }
        private dynamic ApplyFilters(dynamic query, GetTableRequestDTO request)
        {
            if (!string.IsNullOrEmpty(request.Status))
                query = query.Filter("status", Operator.Equals, request.Status);

            if (!string.IsNullOrEmpty(request.TableNumber))
                query = query.Filter("table_number", Operator.ILike, $"%{request.TableNumber}%");

            if (request.MinCapacity.HasValue)
                query = query.Filter("capacity", Operator.GreaterThanOrEqual, request.MinCapacity.Value);

            if (request.MaxCapacity.HasValue)
                query = query.Filter("capacity", Operator.LessThanOrEqual, request.MaxCapacity.Value);

            if (request.TypeId.HasValue)
                query = query.Filter("type_id", Operator.Equals, request.TypeId.Value.ToString());

            return query;
        }

        public async Task<GetTableResponseDTO?> GetTableById(Guid id)
        {
            try
            {
                var tableResponse = await _client
                    .From<Table>()
                    .Where(x => x.Id == id)
                    .Single();

                if (tableResponse == null)
                {
                    throw new Exception("Table not found!");
                }
                string tableTypeName = "Unknown";
                if (await IsTypeExists(tableResponse.TypeId))
                {
                    var tableTypeResponse = await _client
                        .From<TableType>()
                        .Where(x => x.Id == tableResponse.TypeId)
                        .Single();

                    tableTypeName = tableTypeResponse?.Name ?? "Unknown";
                }
                var tableDetail = _mapper.Map<GetTableResponseDTO>(tableResponse);
                tableDetail.TableType = tableTypeName;
                return tableDetail;
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
        public async Task<bool> IsTypeExists(Guid typeId)
        {
            try
            {
                var response = await _client
                    .From<TableType>()
                    .Where(x => x.Id == typeId)
                    .Single();

                return response != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking type existence: {ex.Message}");
            }
        }

        public async Task<GetTableResponseDTO> CreateTable(CreateTableRequestDTO createTable)
        {
            if (!await IsShopExists(createTable.ShopId))
                throw new Exception("Shop ID does not exist!");

            if (!await IsTypeExists(createTable.TypeId))
                throw new Exception("Type ID does not exist!");

            if (!await GetTableByNumber(createTable.TableNumber, createTable.ShopId))
                throw new Exception("Table Number already exists!");

            var table = _mapper.Map<Table>(createTable);
            table.Id = Guid.NewGuid();
            table.CreatedAt = DateTime.UtcNow;
            table.UpdatedAt = DateTime.UtcNow;

            var response = await _client.From<Table>().Insert(table);
            if (response == null)
                throw new Exception("Error! Insert error!");

            return await GetTableById(table.Id);
        }

        public async Task<GetTableResponseDTO> UpdateTable(Guid id, UpdateTableRequestDTO updateTable)
        {
            var existingTable = await _client.From<Table>().Where(x => x.Id == id).Single();
            if (existingTable == null)
                throw new Exception("Table not found!");

            _mapper.Map(updateTable, existingTable);
            existingTable.UpdatedAt = DateTime.UtcNow;

            var updateResponse = await _client.From<Table>().Where(x => x.Id == id).Update(existingTable);
            if (updateResponse == null)
                throw new Exception("Error! Update error!");

            return await GetTableById(existingTable.Id);
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

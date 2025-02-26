using AutoMapper;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Supabase;

using static Supabase.Postgrest.Constants;

namespace BusinessObject.Services
{
    public class TableDAO
    {
        private readonly Client _client;
        private readonly IMapper _mapper;

        public TableDAO(Client client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }


        public async Task<(List<GetTableResponseDTO> Tables, TablePaginationDTO PaginationMetadata)> GetAllTables(GetTableRequestDTO request)

        {
            try
            {
                //Get list of all table and apply filters
                var query = _client.From<Table>().Select("*");
                query = ApplyFilters(query, request);

                //Also apply filters but for counting since if using .Count it will reset filters
                var counting = _client.From<Table>().Select("*");
                counting = ApplyFilters(counting, request);
                var totalRecordsResponse = await counting.Select("id").Get();
                var totalRecords = totalRecordsResponse.Models?.Count ?? 0;
                var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);


                //Count for skipping page page, if page 1 then skip 0 page, if page 2-> skip 1 page
                var skip = (request.PageNumber - 1) * request.PageSize;
                var paginatedQuery = query.Range(skip, skip + request.PageSize - 1);

                //List of the request page

                var tablesResponse = await paginatedQuery.Get();
                var tableTypesResponse = await _client.From<TableType>().Select("*").Get();
                var typeNameList = tableTypesResponse.Models.ToDictionary(tt => tt.Id, tt => tt.Name);

                //Set tabletype name to list of dto since table only have id 
                var tables = tablesResponse.Models
                    .Select(table =>
                    {
                        var dto = _mapper.Map<GetTableResponseDTO>(table);
                        dto.TableType = typeNameList.ContainsKey(table.TypeId) ? typeNameList[table.TypeId] : "Unknown";
                        return dto;
                    })
                    .ToList();

                //If there no item or error of page number, return emty
                if (totalRecords == 0 || request.PageNumber > totalPages)
                {
                    return (
                        new List<GetTableResponseDTO>(),
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

                return (tables, paginationMetadata);
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
        private dynamic ApplyFilters(dynamic query, GetTablesRequestDTO request)
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
                    return null;
                }
                string tableTypeName = "Unknown";
                var tableTypeResponse = await _client
                        .From<TableType>()
                        .Where(x => x.Id == tableResponse.TypeId)
                        .Single();
                tableTypeName = tableTypeResponse?.Name ?? "Unknown";

                var tableDetail = _mapper.Map<GetTableResponseDTO>(tableResponse);
                tableDetail.TableType = tableTypeName;
                return tableDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<bool> isTableExistInShop(string number, Guid shopId)
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
                return response != null;
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

        public async Task<CUDTableResponseDTO> CreateTable(CreateTableRequestDTO createTable)
        {
            if (!await IsShopExists(createTable.ShopId))
                return new CUDTableResponseDTO { Success = false, Message = "Shop ID does not exist!" };

            if (!await IsTypeExists(createTable.TypeId))
                return new CUDTableResponseDTO { Success = false, Message = "Type ID does not exist!" };

            if (await isTableExistInShop(createTable.TableNumber, createTable.ShopId))
                return new CUDTableResponseDTO { Success = false, Message = "Table Number already exists!" };

            var table = _mapper.Map<Table>(createTable);
            table.Id = Guid.NewGuid();
            table.CreatedAt = DateTime.UtcNow;
            table.UpdatedAt = DateTime.UtcNow;
            table.TypeId = createTable.TypeId;

            var response = await _client.From<Table>().Insert(table);
           
            if (response == null || response.Models == null || response.Models.Count == 0)
                return new CUDTableResponseDTO { Success = false, Message = "Error! Insert failed!" };
            var insertedTable = response.Models.First();
            
            return new CUDTableResponseDTO
            {
                Success = true,
                Message = "Table created successfully!",
                Data = await GetTableById(insertedTable.Id),
            };
        }

        public async Task<CUDTableResponseDTO> UpdateTable(Guid id, UpdateTableRequestDTO updateTable)
        {
            var existingTable = await _client.From<Table>().Where(x => x.Id == id).Single();
            if (existingTable == null)
                return new CUDTableResponseDTO { Success = false, Message = "Table not found!" };

            _mapper.Map(updateTable, existingTable);
            existingTable.UpdatedAt = DateTime.UtcNow;

            var updateResponse = await _client.From<Table>().Where(x => x.Id == id).Update(existingTable);
            if (updateResponse == null)
                return new CUDTableResponseDTO { Success = false, Message = "Error! Update failed!" };
           
            var insertedTable = updateResponse.Models.First();
            var updatedTable = await GetTableById(insertedTable.Id);

            return new CUDTableResponseDTO
            {
                Success = true,
                Message = "Table updated successfully!",
                Data = updatedTable
            };
        }
        public async Task<CUDTableResponseDTO> DeleteTable(Guid id)
        {
            try
            {
                var existingTable = await _client.From<Table>().Where(x => x.Id == id).Single();
                if (existingTable == null)
                {
                    return new CUDTableResponseDTO
                    {
                        Success = false,
                        Message = "Table not found!"
                    };
                }
                // Kiểm tra xem table có xuất hiện trong Bill không
                var hasBill = await _client.From<Bill>().Where(b => b.TableId == id).Get();
                if (hasBill !=null)
                {
                    // Nếu có bill, chỉ cập nhật trạng thái
                    existingTable.Status = "Deleted";  // Hoặc Enum Status.Deleted
                    existingTable.UpdatedAt = DateTime.UtcNow;
                    await _client.From<Table>().Where(x => x.Id == id).Update(existingTable);

                    return new CUDTableResponseDTO
                    {
                        Success = true,
                        Message = "Table has active bills. Status updated to 'Deleted'."
                    };
                }
                else
                {
                    // Không có bill thì xóa hoàn toàn
                    await _client.From<Table>().Where(x => x.Id == id).Delete();

                    return new CUDTableResponseDTO
                    {
                        Success = true,
                        Message = "Table successfully deleted."
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }


        public async Task<UpdateTableStatusResponseDTO> UpdateTableStatus(Guid tableId, bool isFinish)
        {
            try
            {
                var table = await _client.From<Table>()
                                         .Where(t => t.Id == tableId)
                                         .Single();

                if (table == null)
                {
                    return new UpdateTableStatusResponseDTO
                    {
                        Success = false,
                        Message = "Table not found."
                    };
                }

                // Chỉ có thể `finish` hoặc `cancel` nếu bàn đang `onusing`
                if (table.Status != "onusing")
                {
                    return new UpdateTableStatusResponseDTO
                    {
                        Success = false,
                        Message = "Table is not in use, so it cannot be canceled or finished."
                    };
                }

                if (isFinish)
                {
                    // Hoàn tất đơn, đổi trạng thái bàn và in hóa đơn
                    table.Status = "available";
                    await _client.From<Table>().Update(table);

                    var billDAO = new BillDAO(_client);
                    var billId = await billDAO.GetBillIdByTableId(tableId);
                    var billPath = await billDAO.GenerateAndPrintBillPdf(billId);

                    return new UpdateTableStatusResponseDTO
                    {
                        Success = true,
                        Message = "Table booking has been finished and bill printed.",
                        BillPath = billPath
                    };
                }
                else
                {
                    //  Hủy đặt bàn
                    table.Status = "available";
                    await _client.From<Table>().Update(table);

                    return new UpdateTableStatusResponseDTO
                    {
                        Success = true,
                        Message = "Table booking has been canceled."
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }


    }
}

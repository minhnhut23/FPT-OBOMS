using BusinessObject.DTOs.TableDTO;
using BusinessObject.Services;
using BusinessObject.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TableController : ControllerBase
    {
        private readonly TableDAO _tableDAO;

        public TableController(TableDAO tableService)
        {
            _tableDAO = tableService;
        }

        [HttpGet("tables")]
        public async Task<IActionResult> GetAllTables([FromQuery] GetTableRequestDTO request)
        {
            try
            {
                // Call the service method
                var (tables, paginationMetadata) = await _tableDAO.GetAllTables(request);

                // Prepare the response object
                var response = new GetAllTablesResponseDTO
                {
                    Data = tables,
                    Pagination = paginationMetadata
                };

                return Ok(response); // Return 200 OK with data
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, new { Message = ex.Message }); // Return 500 Internal Server Error
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableById(Guid id)
        {
            try
            {
                var table = await _tableDAO.GetTableById(id);
                if (table == null) return NotFound("Table not found.");
                return Ok(table);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable([FromBody] CreateTableRequestDTO createTable)
        {
            try
            {
               
                if (createTable == null)
                {
                    return BadRequest("Invalid data provided.");
                }
               
                if (!await _tableDAO.GetTableByNumber(createTable.TableNumber, createTable.ShopId))
                {
                    return Conflict("Table number already exists for this shop.");
                }

                if (createTable.Capacity <= 0)
                {
                    return BadRequest("Capacity must be a positive number.");
                }
                var createdTable = await _tableDAO.CreateTable(createTable);
                return Ok($"Created successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTable(Guid id, [FromBody] UpdateTableRequestDTO updateTable)
        {
            try
            {
                if (updateTable == null)
                {
                    return BadRequest("Invalid data.");
                }
                if (await _tableDAO.GetTableById(id) == null)
                {
                    return NotFound("Table not found.");
                }
                if (updateTable.Capacity <= 0)
                {
                    return BadRequest("Capacity must be a positive number.");
                }
                var updatedTable = await _tableDAO.UpdateTable(id, updateTable);
                return Ok(updatedTable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(Guid id)
        {
            try
            {
                var deleted = await _tableDAO.DeleteTable(id);
                if (!deleted.IsDeleted) return BadRequest(deleted.Message);
                return Ok(deleted.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
    }
}

using BusinessObject.DTOs.TableDTO;
using BusinessObject.Services;
using BusinessObject.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagementService.IRepositories;


namespace ShopManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableRepository _tableRepository;

        public TableController(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        [HttpGet("tables")]
        public async Task<IActionResult> GetAllTables([FromQuery] GetTableRequestDTO request)
        {
            try
            {
                var (tables, paginationMetadata) = await _tableRepository.GetAllTables(request);

                var response = new
                {
                    Data = tables,
                    Pagination = paginationMetadata
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableById(Guid id)
        {
            try
            {
                var table = await _tableRepository.GetTableById(id);
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

                if (!await _tableRepository.GetTableByNumber(createTable.TableNumber, createTable.ShopId))
                {
                    return Conflict("Table number already exists for this shop.");
                }

                if (createTable.Capacity <= 0)
                {
                    return BadRequest("Capacity must be a positive number.");
                }

                var createdTable = await _tableRepository.CreateTable(createTable);
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

                if (await _tableRepository.GetTableById(id) == null)
                {
                    return NotFound("Table not found.");
                }

                if (updateTable.Capacity <= 0)
                {
                    return BadRequest("Capacity must be a positive number.");
                }

                var updatedTable = await _tableRepository.UpdateTable(id, updateTable);
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
                var deleted = await _tableRepository.DeleteTable(id);
                if (!deleted.IsDeleted) return BadRequest(deleted.Message);
                return Ok(deleted.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTableStatus(Guid id, [FromQuery] bool isFinish)
        {
            var result = await _tableRepository.UpdateTableStatus(id, isFinish);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
    }
}

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
        public async Task<IActionResult> GetAllTables([FromQuery] GetTablesRequestDTO request)
        {
            try
            {
                var (tables, paginationMetadata) = await _tableRepository.GetAllTables(request);

                if (tables.Count == 0)
                {
                    return NotFound(new { Message = "No tables found matching the criteria." });
                }

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
                    return BadRequest(new { Message = "Invalid data provided." });

                if (createTable.Capacity <= 0)
                    return BadRequest(new { Message = "Capacity must be a positive number." });

                var createdTable = await _tableRepository.CreateTable(createTable);
                if (!createdTable.Success) return BadRequest(createdTable.Message);
                return Ok(createdTable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ErrorHandler.ProcessErrorMessage(ex.Message) });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTable(Guid id, [FromBody] UpdateTableRequestDTO updateTable)
        {
            try
            {
                if (updateTable == null)
                {
                    return BadRequest(new CUDTableResponseDTO
                    {
                        Success = false,
                        Message = "Invalid data."
                    });
                }

                if (updateTable.Capacity <= 0)
                {
                    return BadRequest(new CUDTableResponseDTO
                    {
                        Success = false,
                        Message = "Capacity must be a positive number."
                    });
                }

                var response = await _tableRepository.UpdateTable(id, updateTable);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CUDTableResponseDTO
                {
                    Success = false,
                    Message = ErrorHandler.ProcessErrorMessage(ex.Message)
                });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(Guid id)
        {
            try
            {
                var response = await _tableRepository.DeleteTable(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CUDTableResponseDTO
                {
                    Success = false,
                    Message = ErrorHandler.ProcessErrorMessage(ex.Message)
                });
            }
        }

/*
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTableStatus(Guid id, [FromQuery] bool isFinish)
        {
            var result = await _tableRepository.UpdateTableStatus(id, isFinish);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }*/
    }
}

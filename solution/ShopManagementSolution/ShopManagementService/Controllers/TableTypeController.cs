using BusinessObject.DTOs.TableTypeDTO;
using BusinessObject.Enums;
using BusinessObject.Utils;
using Microsoft.AspNetCore.Mvc;
using ShopManagementService.IRepositories;
using ShopManagementService.Utils.Security;
using System;
using System.Threading.Tasks;

namespace ShopManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableTypeController : ControllerBase
    {
        private readonly ITableTypeRepository _tableTypeRepository;

        public TableTypeController(ITableTypeRepository tableTypeRepository)
        {
            _tableTypeRepository = tableTypeRepository;
        }

        // Get all Table Types
        [HttpGet]
        [AuthorizeRole(UserRole.Owner)]
        public async Task<IActionResult> GetAllTableTypes()
        {
            try
            {
                var tableTypes = await _tableTypeRepository.GetAllTableTypes();
                return Ok(tableTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        // Get Table Type by ID
        [HttpGet("{id}")]
        [AuthorizeRole(UserRole.Owner)]
        public async Task<IActionResult> GetTableTypeById(Guid id)
        {
            try
            {
                var tableType = await _tableTypeRepository.GetTableTypeById(id);
                if (tableType == null) return NotFound("Table Type not found.");
                return Ok(tableType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        // Create Table Type
        [HttpPost]
        [AuthorizeRole(UserRole.Owner)]
        public async Task<IActionResult> CreateTableType([FromBody] AddEditTypeRequestDTO createTableType)
        {
            try
            {
                if (createTableType == null || string.IsNullOrWhiteSpace(createTableType.Name))
                {
                    return BadRequest("Invalid data provided.");
                }

                var createdTableType = await _tableTypeRepository.CreateTableType(createTableType);
                return Ok(createdTableType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        // Update Table Type
        [HttpPut("{id}")]
        [AuthorizeRole(UserRole.Owner)]
        public async Task<IActionResult> UpdateTableType(Guid id, [FromBody] AddEditTypeRequestDTO updateTableType)
        {
            try
            {
                if (updateTableType == null || string.IsNullOrWhiteSpace(updateTableType.Name))
                {
                    return BadRequest("Invalid data.");
                }

                var existingTableType = await _tableTypeRepository.GetTableTypeById(id);
                if (existingTableType == null)
                {
                    return NotFound("Table Type not found.");
                }

                var updatedTableType = await _tableTypeRepository.UpdateTableType(id, updateTableType);
                return Ok(updatedTableType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        // Delete Table Type
        [HttpDelete("{id}")]
        [AuthorizeRole(UserRole.Owner)]
        public async Task<IActionResult> DeleteTableType(Guid id)
        {
            try
            {
                var deleted = await _tableTypeRepository.DeleteTableType(id);
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

using BusinessObject.Models;
using BusinessObject.DTOs.ShopDTO;
using BusinessObject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ShopManagementService.IRepositories;
using BusinessObject.DTOs.TableDTO;
using Microsoft.AspNetCore.Authorization;
using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using ShopManagementService.Utils.Security;
using BusinessObject.Enums;

namespace BusinessObject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopRepository _repo;

        public ShopController(IShopRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShops([FromQuery] GetShopRequestDTO request)
        {
            try
            {
                var shops = await _repo.GetAllShops(request);
                return Ok(shops);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShopById(Guid id)
        {
            try
            {
                var shop = await _repo.GetShopById(id);
                if (shop == null)
                {
                    return NotFound("Shop not found.");
                }

                return Ok(shop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRole.Owner)]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopRequestDTO createShop)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");


                var createdShop = await _repo.CreateShop(createShop, token);
                return CreatedAtAction(nameof(GetShopById), new { id = createdShop.Id }, createdShop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AuthorizeRole(UserRole.Owner)]
        public async Task<IActionResult> UpdateShop(Guid id, [FromBody] UpdateShopRequestDTO updateShop)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");


                var updatedShop = await _repo.UpdateShop(id, updateShop, token);
                if (updatedShop == null)
                {
                    return NotFound("Shop not found.");
                }

                return Ok(updatedShop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(Guid id)
        {
            try
            {
                var result = await _repo.DeleteShop(id);
                if (!result.IsDeleted)
                {
                    return NotFound(result.Message);
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

using BusinessObject.Models;
using BusinessObject.DTOs.ShopDTO;
using BusinessObject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ShopManagementService.IRepositories;

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
        public async Task<IActionResult> GetAllShops()
        {
            try
            {
                var shops = await _repo.GetAllShops();
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
        public async Task<IActionResult> CreateShop([FromBody] CreateShopRequestDTO createShop)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var createdShop = await _repo.CreateShop(createShop);
                return CreatedAtAction(nameof(GetShopById), new { id = createdShop.Id }, createdShop);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(Guid id, [FromBody] UpdateShopRequestDTO updateShop)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var updatedShop = await _repo.UpdateShop(id, updateShop);
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

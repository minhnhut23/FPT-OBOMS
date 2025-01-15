using BusinessObject.DTOs.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using ShopManagementService.DAO;
using ShopManagementService.Repositories;

namespace ShopManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _repo;

        public ProductController(ProductRepository repo)
        {
            _repo = repo;
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await _repo.GetProductById(id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDTO request)
        {
            try
            {
                await _repo.CreateProduct(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequestDTO request)
        {
            try
            {
                await _repo.UpdateProduct(request, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _repo.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}

using BusinessObject.DTOs.BillDetailDTO;
using BusinessObject.DTOs.BillDTO;
using Microsoft.AspNetCore.Mvc;
using ShopManagementService.IRepositories;
using System;
using System.Threading.Tasks;

namespace BusinessObject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillDetailController : ControllerBase
    {
        private readonly IBillDetailRepository _billDetailRepository;

        public BillDetailController(IBillDetailRepository billDetailRepository)
        {
            _billDetailRepository = billDetailRepository;
        }

       
        // GET: api/billdetail/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillDetailById(Guid id)
        {
            try
            {
                var billDetail = await _billDetailRepository.GetBillDetailByID(id);
                if (billDetail == null)
                {
                    return NotFound("Bill detail not found.");
                }

                return Ok(billDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/billdetail
        [HttpPost]
        public async Task<IActionResult> CreateBillDetail([FromBody] CreateBillDetailRequestDTO createBillDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var createdBillDetail = await _billDetailRepository.CreateBillDetail(createBillDetail);
                return CreatedAtAction(nameof(GetBillDetailById), new { id = createdBillDetail.Id }, createdBillDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/billdetail/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBillDetail(Guid id, [FromBody] UpdateBillDetailRequestDTO updateBillDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var updatedBillDetail = await _billDetailRepository.UpdateBillDetail(id, updateBillDetail);
                return Ok(updatedBillDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/billdetail/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillDetail(Guid id)
        {
            try
            {
                var result = await _billDetailRepository.DeleteBillDetail(id);
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

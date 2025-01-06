using BusinessObject.DTOs.BillDTO;
using BusinessObject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BusinessObject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillDAO _billDao;

        public BillController(BillDAO billDao)
        {
            _billDao = billDao;
        }

        // GET: api/bill
        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {
            try
            {
                var bills = await _billDao.GetAllBills();
                return Ok(bills);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/bill/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillById(Guid id)
        {
            try
            {
                var bill = await _billDao.GetBillById(id);
                if (bill == null)
                {
                    return NotFound("Bill not found.");
                }

                return Ok(bill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/bill
        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] CreateBillRequestDTO createBill)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var createdBill = await _billDao.CreateBill(createBill);
                return CreatedAtAction(nameof(GetBillById), new { id = createdBill.Id }, createdBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/bill/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBill(Guid id, [FromBody] UpdateBillRequestDTO updateBill)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var updatedBill = await _billDao.UpdateBill(id, updateBill);
                return Ok(updatedBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/bill/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(Guid id)
        {
            try
            {
                var result = await _billDao.DeleteBill(id);
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

using BusinessObject.DTOs.BillDTO;
using ShopManagementService.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BusinessObject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillRepository _billRepository;

        public BillController(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {
            try
            {
                var bills = await _billRepository.GetAllBills();
                return Ok(bills);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillByID(Guid id)
        {
            try
            {
                var bill = await _billRepository.GetBillByID(id);
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

       
        [HttpPost]
        public async Task<IActionResult> CreateBill([FromBody] CreateBillRequestDTO createBill)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var createdBill = await _billRepository.CreateBill(createBill);
                return CreatedAtAction(nameof(GetBillByID), new { id = createdBill.Id }, createdBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBill(Guid id,UpdateBillRequestDTO updateBill)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid input data.");
                }

                var updatedBill = await _billRepository.UpdateBill(id, updateBill);
                return Ok(updatedBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(Guid id)
        {
            try
            {
                var result = await _billRepository.DeleteBill(id);
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

        [HttpGet("{id}/bill")]
        public async Task<IActionResult> GetBillByTableID(Guid tableId)
        {
            var billId = await _billRepository.GetBillByTableID(tableId);
            if (billId == null)
                return NotFound("There are no bill of this table.");

            return Ok(new { billId });
        }


        [HttpGet("generate-pdf/{id}")]
        public async Task<IActionResult> GenerateAndPrintBillPdf(Guid id)
        {
            try
            {
                string filePath = await _billRepository.GenerateAndPrintBillPdf(id);


                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { message = "File not found." });
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

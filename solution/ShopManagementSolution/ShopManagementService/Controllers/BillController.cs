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
        [HttpGet("draft/{id}")]
        public async Task<IActionResult> GetDraftBillById(Guid id)
        {
            try
            {
                var draftBill = await _billDao.GetDraftBillById(id);
                if (draftBill == null)
                {
                    return NotFound(new { message = "Draft bill not found." });
                }

                return Ok(draftBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
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

                var createdBill = await _billDao.CreateBill(createBill);
                return CreatedAtAction(nameof(GetBillById), new { id = createdBill.Id }, createdBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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

        [HttpGet("generate-pdf/{id}")]
        public async Task<IActionResult> GenerateBillPdf(Guid id)
        {
            try
            {
                // Gọi phương thức GenerateBillPdfById từ DAO và nhận lại filePath
                string filePath = await _billDao.GenerateBillPdfById(id);

                // Kiểm tra xem file có tồn tại không
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { message = "File not found." });
                }

                // Đọc nội dung file PDF
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Trả về file PDF dưới dạng response
                return File(fileBytes, "application/pdf", Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}

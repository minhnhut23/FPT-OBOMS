using BusinessObject.DTOs.BillDetailDTO;
using BusinessObject.DTOs.BillDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Exceptions;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Diagnostics;
using System.Reflection.Metadata;
using ZXing;  // Thư viện tạo QR code
namespace BusinessObject.Services
{
    public class BillDAO
    {
        private readonly Supabase.Client _client;

        public BillDAO(Supabase.Client client)
        {
            _client = client;
        }

        public async Task<List<BillResponseDTO>> GetAllBills()
        {
            try
            {
                var billResponse = await _client.From<Bill>().Get();
                var bills = billResponse.Models;

                var billDetailsResponse = await _client.From<BillDetail>().Get();
                var billDetailsDict = billDetailsResponse.Models
                    .GroupBy(bd => bd.BillId)
                    .ToDictionary(g => g.Key, g => g.Sum(bd => bd.Quantity));

                return bills.Select(bill => new BillResponseDTO
                {
                    Id = bill.Id,
                    ReservationId = bill.ReservationId,
                    TotalAmount = bill.TotalAmount,
                    ReceivedAmount = bill.ReceivedAmount,
                    ChangeAmount = bill.ChangeAmount,
                    TableId = bill.TableId,
                    CreatedAt = bill.CreatedAt,
                    UpdatedAt = bill.UpdatedAt,
                    CustomerId = bill.CustomerId,
                    ShopId = bill.ShopId,
                    BillDetailsQuantity = billDetailsDict.ContainsKey(bill.Id) ? billDetailsDict[bill.Id] : 0
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<BillWithDetailsResponseDTO?> GetDraftBillById(Guid id)
        {
            try
            {
                var billResponse = await _client.From<Bill>().Where(b => b.Id == id).Single();
                if (billResponse == null)
                    return null;

                var billDetailsResponse = await _client.From<BillDetail>().Where(bd => bd.BillId == id).Get();

                var billDetailsDTOs = billDetailsResponse.Models.Select(bd => new BillDetailResponseDTO
                {
                    Id = bd.Id,
                    BillId = bd.BillId,
                    MenuItemId = bd.MenuItemId,
                    Quantity = bd.Quantity,
                    Price = bd.Price,
                    CreatedAt = bd.CreatedAt,
                    UpdatedAt = bd.UpdatedAt
                }).ToList();

                return new BillWithDetailsResponseDTO
                {
                    Id = billResponse.Id,
                    ReservationId = billResponse.ReservationId,
                    TotalAmount = billResponse.TotalAmount,
                    TableId = billResponse.TableId,
                    CreatedAt = billResponse.CreatedAt,
                    UpdatedAt = billResponse.UpdatedAt,
                    CustomerId = billResponse.CustomerId,
                    ShopId = billResponse.ShopId,
                    BillDetails = billDetailsDTOs
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
         public async Task<BillWithDetailsResponseDTO?> GetBillById(Guid id)
        {
            try
            {
                var billResponse = await _client.From<Bill>().Where(b => b.Id == id).Single();
                if (billResponse == null)
                    return null;

                var billDetailsResponse = await _client.From<BillDetail>().Where(bd => bd.BillId == id).Get();

                var billDetailsDTOs = billDetailsResponse.Models.Select(bd => new BillDetailResponseDTO
                {
                    Id = bd.Id,
                    BillId = bd.BillId,
                    MenuItemId = bd.MenuItemId,
                    Quantity = bd.Quantity,
                    Price = bd.Price,
                    CreatedAt = bd.CreatedAt,
                    UpdatedAt = bd.UpdatedAt
                }).ToList();

                return new BillWithDetailsResponseDTO
                {
                    Id = billResponse.Id,
                    ReservationId = billResponse.ReservationId,
                    TotalAmount = billResponse.TotalAmount,
                    ReceivedAmount = billResponse.ReceivedAmount,
                    ChangeAmount = billResponse.ChangeAmount,
                    TableId = billResponse.TableId,
                    CreatedAt = billResponse.CreatedAt,
                    UpdatedAt = billResponse.UpdatedAt,
                    CustomerId = billResponse.CustomerId,
                    ShopId = billResponse.ShopId,
                    BillDetails = billDetailsDTOs
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<BillResponseDTO> CreateBill(CreateBillRequestDTO createBill)
        {
            try
            {
                var bill = new Bill
                {
                    Id = Guid.NewGuid(),
                    ReservationId = createBill.ReservationId,
                    TotalAmount = createBill.TotalAmount,
                    ReceivedAmount = createBill.ReceivedAmount,
                    ChangeAmount = createBill.ChangeAmount,
                    TableId = createBill.TableId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CustomerId = createBill.CustomerId,
                    ShopId = createBill.ShopId
                };

                var response = await _client.From<Bill>().Insert(bill);

                return new BillResponseDTO
                {
                    Id = bill.Id,
                    ReservationId = bill.ReservationId,
                    TotalAmount = bill.TotalAmount,
                    ReceivedAmount = bill.ReceivedAmount,
                    ChangeAmount = bill.ChangeAmount,
                    TableId = bill.TableId,
                    CreatedAt = bill.CreatedAt,
                    UpdatedAt = bill.UpdatedAt,
                    CustomerId = bill.CustomerId,
                    ShopId = bill.ShopId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
        public async Task<string> GenerateBillPdfById(Guid billId)
        {
            try
            {
                // Lấy dữ liệu hóa đơn theo ID
                var bill = await GetBillById(billId);  // Hàm này trả về đối tượng BillWithDetailsResponseDTO

                if (bill == null)
                {
                    throw new Exception("Bill not found.");
                }

                // Tạo tên file PDF từ ID hóa đơn
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Bills");  // Thư mục "Bills" trong thư mục hiện tại
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);  // Tạo thư mục nếu chưa có
                }

                string filePath = Path.Combine(folderPath, $"Bill_{billId}.pdf");
                using (var writer = new PdfWriter(filePath))
                using (var pdf = new PdfDocument(writer))
                using (var document = new iText.Layout.Document(pdf))
                {
                    document.Add(new Paragraph("Testing PDF generation"));

                    // Đảm bảo đóng PDF
                    document.Close();
                }

                // Kiểm tra file đã được tạo thành công
                if (!File.Exists(filePath))
                {
                    throw new Exception("File was not created successfully.");
                }

                return filePath;
            }
            catch (PdfException ex)
            {
                throw new Exception($"PDF generation failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating PDF: {ex.Message}", ex);
            }
        }

        private void OpenAndPrintPdf(string filePath)
        {
            try
            {
                // Mở file PDF trong ứng dụng mặc định của hệ thống
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true,  // Mở với ứng dụng mặc định (ví dụ: Adobe Reader)
                    Verb = "print"  // Gửi lệnh in
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error opening and printing PDF: {ex.Message}");
            }
        }
        public async Task<BillResponseDTO> UpdateBill(Guid id, UpdateBillRequestDTO updateBill)
        {
            try
            {
                var bill = await _client
                    .From<Bill>()
                    .Where(x => x.Id == id)
                    .Single();

                if (bill == null) throw new Exception("Bill not found.");

                bill.TotalAmount = updateBill.TotalAmount;
                bill.ReceivedAmount = updateBill.ReceivedAmount;
                bill.ChangeAmount = updateBill.ChangeAmount;
                bill.TableId = updateBill.TableId;
                bill.UpdatedAt = DateTime.UtcNow;

                var updatedBill = await _client
                    .From<Bill>()
                    .Where(x => x.Id == id)
                    .Update(bill);

                return new BillResponseDTO
                {
                    Id = bill.Id,
                    ReservationId = bill.ReservationId,
                    TotalAmount = bill.TotalAmount,
                    ReceivedAmount = bill.ReceivedAmount,
                    ChangeAmount = bill.ChangeAmount,
                    TableId = bill.TableId,
                    CreatedAt = bill.CreatedAt,
                    UpdatedAt = bill.UpdatedAt,
                    CustomerId = bill.CustomerId,
                    ShopId = bill.ShopId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<DeleteBillResponseDTO> DeleteBill(Guid id)
        {
            try
            {
                var bill = await GetBillById(id);
                if (bill == null)
                    return new DeleteBillResponseDTO { IsDeleted = false, Message = "Bill not found." };

                await _client
                    .From<Bill>()
                    .Where(x => x.Id == id)
                    .Delete();

                return new DeleteBillResponseDTO
                {
                    IsDeleted = true,
                    Message = "Bill successfully deleted."
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }
    }
}

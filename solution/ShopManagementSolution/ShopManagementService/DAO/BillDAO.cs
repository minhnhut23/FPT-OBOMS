using BusinessObject.DTOs.BillDetailDTO;
using BusinessObject.DTOs.BillDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using static Supabase.Postgrest.Constants;
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

                var billDetailsDTO = billDetailsResponse.Models.Select(bd => new BillDetailResponseDTO
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
                    BillDetails = billDetailsDTO
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<Guid> GetBillIdByTableId(Guid tableId)
        {
            try
            {
                var billResponse = await _client
                    .From<Bill>()
                    .Where(b => b.TableId == tableId)
                    .Order("created_at", Ordering.Descending) // Lấy hóa đơn mới nhất
                    .Single();

                return billResponse.Id;
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

        public bool IsFileLocked(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // If file can be opened, it's not locked
                    return false;
                }
            }
            catch (IOException)
            {
                // If an exception is thrown, the file is locked
                return true;
            }
        }

        public async Task<string> GenerateAndPrintBillPdf(Guid billId)
        {
            try
            {
                var bill = await GetBillById(billId);
                if (bill == null)
                    throw new Exception("Bill not found.");

                // Ensure the Bills folder exists
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Bills");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, $"Bill_{bill.Id}.pdf");

                if (IsFileLocked(filePath))
                {
                    Console.WriteLine($"File {filePath} is locked by another process.");
                }

                float pageWidth = 400f; // Chiều rộng cố định
                using (var writer = new PdfWriter(filePath))
                using (var pdf = new PdfDocument(writer))
                {
                    // Thiết lập chiều rộng cố định, chiều cao tự động
                    var pageSize = new iText.Kernel.Geom.PageSize(pageWidth, iText.Kernel.Geom.PageSize.A4.GetHeight());

                    pdf.SetDefaultPageSize(pageSize);

                    using (var document = new iText.Layout.Document(pdf))
                    {
                        // Add content
                        document.Add(new Paragraph("Invoice")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontSize(20));

                        document.Add(new Paragraph($"Bill ID: {bill.Id}")
                            .SetTextAlignment(TextAlignment.LEFT));
                        document.Add(new Paragraph($"Customer ID: {bill.CustomerId}")
                            .SetTextAlignment(TextAlignment.LEFT));
                        document.Add(new Paragraph($"Shop ID: {bill.ShopId}")
                            .SetTextAlignment(TextAlignment.LEFT));
                        document.Add(new Paragraph($"Total Amount: {bill.TotalAmount:C}")
                            .SetTextAlignment(TextAlignment.LEFT));

                        // Add separator
                        document.Add(new Paragraph(new string('-', 40))
                            .SetTextAlignment(TextAlignment.CENTER));

                        // Create and populate table
                        var table = new iText.Layout.Element.Table(4);
                        table.AddHeaderCell("Item Name");
                        table.AddHeaderCell("Quantity");
                        table.AddHeaderCell("Unit Price");
                        table.AddHeaderCell("Total Price");

                        foreach (var item in bill.BillDetails)
                        {
                            table.AddCell(item.Id.ToString());
                            table.AddCell(item.Quantity.ToString());
                            table.AddCell(item.Price.ToString());
                            table.AddCell((item.Quantity * item.Price).ToString());
                        }

                        document.Add(table);
                        document.Add(table);
                        document.Add(table);
                        document.Add(table);
                        document.Add(table);
                        document.Add(table);
                        document.Add(table);
                        document.Add(table);
                        document.Add(table);
                        document.Add(table);

                        // Add notes
                        document.Add(new Paragraph($"Notes: {bill.TotalAmount}")
                            .SetTextAlignment(TextAlignment.LEFT));
                        pageSize = new iText.Kernel.Geom.PageSize(pageWidth, iText.Kernel.Geom.PageSize.A4.GetHeight() * 2);

                        pdf.SetDefaultPageSize(pageSize);
                        // Document auto adjusts the page height
                        document.Close();
                    }



                }

                Console.WriteLine($"PDF generated successfully at: {filePath}");

                // Automatically open and print the PDF
                OpenAndPrintPdf(filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown error: {ex.Message} - {ex.StackTrace}");
                throw new Exception($"Unknown error: {ex.Message} - {ex.StackTrace}", ex);
            }
        }

        private void OpenAndPrintPdf(string filePath)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true,
                        Verb = "print" // Mở chế độ in
                    }
                };
                process.Start();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error opening and printing PDF: {ex.Message}", ex);
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
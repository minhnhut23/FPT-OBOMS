using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using BusinessObject.DTOs.BillDTO;
using System.Diagnostics;

namespace ShopManagementService.Utils
{
    public class PdfService
    {
        public string GenerateBillPdf(BillWithDetailsResponseDTO bill)
        {
            // Đường dẫn lưu file PDF tạm thời
            string filePath = Path.Combine(Path.GetTempPath(), $"Bill_{bill.Id}.pdf");

            // Tạo writer và document
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    // Thêm tiêu đề
                    document.Add(new Paragraph("Invoice")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20));

                    // Thêm thông tin bill
                    document.Add(new Paragraph($"Bill ID: {bill.Id}"));
                    document.Add(new Paragraph($"Reservation ID: {bill.ReservationId}"));
                    document.Add(new Paragraph($"Customer ID: {bill.CustomerId}"));
                    document.Add(new Paragraph($"Shop ID: {bill.ShopId}"));
                    document.Add(new Paragraph($"Table ID: {bill.TableId}"));
                    document.Add(new Paragraph($"Total Amount: {bill.TotalAmount:C}"));
                    document.Add(new Paragraph($"Received Amount: {bill.ReceivedAmount:C}"));
                    document.Add(new Paragraph($"Change Amount: {bill.ChangeAmount:C}"));
                    document.Add(new Paragraph($"Created At: {bill.CreatedAt:dd/MM/yyyy HH:mm}"));
                    document.Add(new Paragraph($"Updated At: {bill.UpdatedAt:dd/MM/yyyy HH:mm}"));

                    // Thêm chi tiết bill
                    document.Add(new Paragraph("Bill Details:"));
                    Table table = new Table(4, true);
                    table.AddHeaderCell("Item");
                    table.AddHeaderCell("Quantity");
                    table.AddHeaderCell("Price");
                    table.AddHeaderCell("Total");

                    foreach (var detail in bill.BillDetails)
                    {
                        table.AddCell(detail.MenuItemId.ToString());
                        table.AddCell(detail.Quantity.ToString());
                        table.AddCell($"{detail.Price:C}");
                        table.AddCell($"{detail.Quantity * detail.Price:C}");
                    }

                    document.Add(table);

                    // Đóng tài liệu
                    document.Close();
                }
            }

            return filePath;
        }
        public void OpenPdfForPrinting(string filePath)
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
    }
}

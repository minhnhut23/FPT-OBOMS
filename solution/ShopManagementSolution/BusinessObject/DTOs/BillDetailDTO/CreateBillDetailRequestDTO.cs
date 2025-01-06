namespace BusinessObject.DTOs.BillDetailDTO
{
    public class CreateBillDetailRequestDTO
    {
        public Guid BillId { get; set; }
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

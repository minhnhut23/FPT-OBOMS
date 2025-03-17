using BusinessObject.DTOs.BillDetailDTO;

namespace BusinessObject.DTOs.BillDTO
{
    public class BillWithDetailsResponseDTO
    {
        public Guid Id { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? ReceivedAmount { get; set; }
        public decimal? ChangeAmount { get; set; }
        public Guid TableId { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid ShopId { get; set; }
        public List<BillDetailResponseDTO> BillDetails { get; set; }
    }
}

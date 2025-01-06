using BusinessObject.DTOs.BillDetailDTO;

namespace BusinessObject.DTOs.BillDTO
{
    public class BillWithDetailsResponseDTO
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ShopId { get; set; }

        // This will hold the list of BillDetails
        public List<BillDetailResponseDTO> BillDetails { get; set; }
    }
}

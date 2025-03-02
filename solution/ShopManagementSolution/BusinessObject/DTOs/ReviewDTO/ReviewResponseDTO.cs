namespace BusinessObject.DTOs.ReviewDTO;

public class ReviewResponseDTO
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}

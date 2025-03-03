namespace BusinessObject.DTOs.ReviewDTO;

public class CreateReviewRequestDTO
{
    public Guid ShopId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;
}

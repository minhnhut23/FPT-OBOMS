namespace BusinessObject.DTOs.ReviewDTO;

public class GetReviewDTO
{
    public string? Rating { get; set; }
    public string SortBy { get; set; } = "Rating";
    public string SortOrder { get; set; } = "desc";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 2;
}

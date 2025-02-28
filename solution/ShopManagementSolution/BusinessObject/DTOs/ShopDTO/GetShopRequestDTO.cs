namespace BusinessObject.DTOs.ShopDTO;

public class GetShopRequestDTO
{
    public string? Status { get; set; }
    public string? ShopName { get; set; }
    public string SortBy { get; set; } = "ShopName";
    public string SortOrder { get; set; } = "asc";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 2;
}

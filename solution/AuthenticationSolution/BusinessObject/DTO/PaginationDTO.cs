namespace BusinessObject.DTO;

public class PaginationDTO
{
    public int TotalResults { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}

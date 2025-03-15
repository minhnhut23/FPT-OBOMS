namespace BusinessObject.DTO;

public class GetProfileRequestDTO
{
    public string? FullName { get; set; }
    public string? Role { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

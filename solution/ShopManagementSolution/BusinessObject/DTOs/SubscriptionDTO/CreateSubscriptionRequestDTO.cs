namespace BusinessObject.DTOs.SubscriptionDTO;

public class CreateSubscriptionRequestDTO
{
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int NumberOfMonths { get; set; }
}

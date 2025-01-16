namespace BusinessObject.DTOs.ProductDTO;

public class UpdateProductRequestDTO
{
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; } = null!;

    public string? Category { get; set; } = null!;

    public string? Ingredient { get; set; } = null!;

    public string? NutritionalIfo { get; set; } = null!;

    public bool IsAvailable { get; set; }
}


namespace BusinessObject.DTOs.ProductDTO
{
    public class GetProductResponseDTO
    {
        public Guid Id { get; set; } // ID của sản phẩm
        public string Name { get; set; } = string.Empty; // Tên sản phẩm
        public decimal Price { get; set; } // Giá sản phẩm
        public string? Description { get; set; } // Mô tả sản phẩm
        public string? Category { get; set; } // Danh mục sản phẩm
        public bool IsAvailable { get; set; } // Tình trạng sẵn có
        public string? Ingredient { get; set; } // Thành phần sản phẩm (nếu có)
        public string? NutritionalInfo { get; set; } // Thông tin dinh dưỡng (nếu có)
        public DateTime CreatedAt { get; set; } // Ngày tạo sản phẩm
        public DateTime UpdatedAt { get; set; } // Ngày cập nhật sản phẩm gần nhất
        public string? Image { get; set; }
    }
}

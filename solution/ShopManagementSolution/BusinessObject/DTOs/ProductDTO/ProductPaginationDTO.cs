namespace BusinessObject.Utils
{
    public class ProductPaginationDTO
    {
        public int TotalResults { get; set; } // Tổng số kết quả
        public int TotalPages { get; set; } // Tổng số trang
        public int CurrentPage { get; set; } // Trang hiện tại
        public int PageSize { get; set; } // Số lượng mục trong mỗi trang
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.ProductDTO
{
    public class GetProductRequestDTO
    {
        public string? Name { get; set; } // Tên sản phẩm cần tìm (tùy chọn)
        public decimal? MinPrice { get; set; } // Giá thấp nhất (tùy chọn)
        public decimal? MaxPrice { get; set; } // Giá cao nhất (tùy chọn)
        public string? Category { get; set; } // Danh mục sản phẩm (tùy chọn)
        public bool? IsAvailable { get; set; } // Tình trạng sẵn có (tùy chọn)
        public int PageNumber { get; set; } = 1; // Số trang (mặc định là 1)
        public int PageSize { get; set; } = 10; // Số lượng sản phẩm mỗi trang (mặc định là 10)
    }
}

using BusinessObject.DTOs.ProductDTO;
using BusinessObject.Utils;
using System.Collections.Generic;

namespace BusinessObject.DTOs.ProductDTO
{
    public class GetAllProductsResponseDTO
    {
        // Danh sách sản phẩm
        public List<GetProductResponseDTO> Data { get; set; }

        // Thông tin phân trang
        public ProductPaginationDTO Pagination { get; set; }
    }
}

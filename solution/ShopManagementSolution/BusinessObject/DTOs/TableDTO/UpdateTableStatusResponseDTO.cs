using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.TableDTO
{
    public class UpdateTableStatusResponseDTO
    {
        public bool Success { get; set; }  // Xác định trạng thái thành công hay thất bại
        public string Message { get; set; } // Thông báo phản hồi
        public string? BillPath { get; set; } // Đường dẫn hóa đơn nếu có (chỉ dùng khi `Finish`)
    }

}

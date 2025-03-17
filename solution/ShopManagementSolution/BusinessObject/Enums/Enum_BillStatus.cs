using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Enums
{
    public enum Enum_BillStatus
    {
        Pending,     // Đang chờ xử lý
        Confirmed,   // Đã xác nhận
        Cancelled,   // Đã hủy
        Completed,   // Đã thanh toán xong
        Draft,       // Hóa đơn nháp
        Refunded,    // Đã hoàn tiền
        Deleted,     // Đã xóa (soft delete)
        Overdue      // Quá hạn thanh toán
    }
}

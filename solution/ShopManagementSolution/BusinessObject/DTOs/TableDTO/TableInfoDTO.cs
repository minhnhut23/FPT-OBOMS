using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class TableInfoDTO
    {
        public Guid Id { get; set; }
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public string LocationDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid ShopId { get; set; }
        public string TableType { get; set; } 



        public string? BilliardType { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public string? ClothMaterial { get; set; }
        public string? FrameMaterial { get; set; }
        public decimal? Price { get; set; }
    }

}

/*
 - Thêm bàn: Select Option Cafe hoặc Billiard ( mặc định Cafe )
    + Nếu Cafe thì nhập thông số của 1 cái bàn bình thường vô
    + Nếu Billiard thì sẽ hiện thêm ô để nhập các thông số riêng mà bàn bida ms có
    ! Validate:
        - Thông tin đầy đủ
        - Mã bàn tồn tại
        - Số vị trí ngồi
        - Giá không âm
        - Chiều dài rộng không âm
        - Bàn billiard phải có type( 3 bi, bida lỗ,... ) 
 - Cập nhật:
    ! Validate: 
        - như trên
        - Số bàn không trùng
 - Xóa: Xóa bàn cho về trạng thái đã xóa hoặc xóa vĩnh viễn (thông tin hóa đơn sẽ để empty hay vẫn để "bàn đã bị xóa"
    ! Validate:
        - Id, mã bàn tồn tại.
 */

/* Thêm loại bàn, cập nhật
    - Ở giao diện quản lí bàn, bấm nút cài đặt -> danh sách category bàn
    - Edit chỉnh sửa hoặc thêm mới tại đây
    ! Validate: 
        - Không trùng tên
 * Xóa loại bàn: Để các bàn ở table type đó về "empty" hoặc status "MissedInfo"
*/
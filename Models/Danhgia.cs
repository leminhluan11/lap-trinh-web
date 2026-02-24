using System;
using System.ComponentModel.DataAnnotations;

namespace YourProjectName.Domain.Entities
{
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }

        public int? MaNguoiDung { get; set; }
        public int? MaSanPham { get; set; }
        public int? MaDonHang { get; set; }

        [Range(1, 5)]
        public int? DiemDanhGia { get; set; }

        [StringLength(255)]
        public string? NoiDung { get; set; }

        [StringLength(255)]
        public string? HinhAnh { get; set; }

        public DateTime? NgayTao { get; set; }

        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual SanPham? SanPham { get; set; }
        public virtual DonHang? DonHang { get; set; }
    }
}
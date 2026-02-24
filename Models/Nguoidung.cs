using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Domain.Entities
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [StringLength(100)]
        public string? TenDangNhap { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? MatKhauMaHoa { get; set; }

        [StringLength(255)]
        public string? HoTen { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [StringLength(50)]
        public string? VaiTro { get; set; }

        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        // Navigation properties
        public virtual ICollection<DiaChi> DiaChis { get; set; } = new List<DiaChi>();
        public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
        public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
        public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
        public virtual ICollection<LichSuKho> LichSuKhos { get; set; } = new List<LichSuKho>();
    }
}
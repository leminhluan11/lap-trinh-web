using System;
using System.ComponentModel.DataAnnotations;

namespace YourProjectName.Domain.Entities
{
    public class LichSuKho
    {
        [Key]
        public int MaLichSu { get; set; }

        public int? MaBienThe { get; set; }
        public int? MaNhanVien { get; set; }

        [StringLength(50)]
        public string? LoaiThayDoi { get; set; }

        public int? SoLuongThayDoi { get; set; }

        [StringLength(100)]
        public string? GhiChu { get; set; }

        public DateTime? NgayTao { get; set; }

        public virtual BienTheSanPham? BienThe { get; set; }
        public virtual NguoiDung? NhanVien { get; set; }
    }
}
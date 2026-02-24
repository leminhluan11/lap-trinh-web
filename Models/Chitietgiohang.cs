using System;
using System.ComponentModel.DataAnnotations;

namespace YourProjectName.Domain.Entities
{
    public class ChiTietGioHang
    {
        [Key]
        public int MaChiTietGio { get; set; }

        public int? MaGioHang { get; set; }
        public int? MaBienThe { get; set; }

        public int? SoLuong { get; set; }

        public DateTime? NgayThem { get; set; }

        public virtual GioHang? GioHang { get; set; }
        public virtual BienTheSanPham? BienThe { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourProjectName.Domain.Entities
{
    public class DiaChi
    {
        [Key]
        public int MaDiaChi { get; set; }

        public int? MaNguoiDung { get; set; }

        [StringLength(255)]
        public string? TenNguoiNhan { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [StringLength(100)]
        public string? TinhThanh { get; set; }

        [StringLength(100)]
        public string? QuanHuyen { get; set; }

        [StringLength(100)]
        public string? PhuongXa { get; set; }

        [StringLength(100)]
        public string? DiaChiChiTiet { get; set; }

        public bool? MacDinh { get; set; }

        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
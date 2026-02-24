using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Domain.Entities
{
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }

        public int? MaNguoiDung { get; set; }
        public int? MaDiaChi { get; set; }
        public int? MaVoucher { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TongTien { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TienGiamGia { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? ThanhTien { get; set; }

        [StringLength(100)]
        public string? PhuongThucThanhToan { get; set; }

        [StringLength(50)]
        public string? TrangThaiThanhToan { get; set; }

        [StringLength(50)]
        public string? TrangThaiDonHang { get; set; }

        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual DiaChi? DiaChi { get; set; }
        public virtual PhieuGiamGia? Voucher { get; set; }

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
    }
}
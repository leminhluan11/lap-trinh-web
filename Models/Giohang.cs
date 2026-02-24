using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Domain.Entities
{
    public class BienTheSanPham
    {
        [Key]
        public int MaBienThe { get; set; }

        public int? MaSanPham { get; set; }

        [StringLength(100)]
        [Index(IsUnique = true, Name = "UK_SKU")]
        public string? MaSku { get; set; }

        [StringLength(50)]
        public string? KichThuoc { get; set; }

        [StringLength(50)]
        public string? MauSac { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Required]
        public decimal GiaBan { get; set; }

        [Required]
        public int SoLuongTonKho { get; set; }

        [StringLength(255)]
        public string? HinhAnh { get; set; }

        public virtual SanPham? SanPham { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
        public virtual ICollection<LichSuKho> LichSuKhos { get; set; } = new List<LichSuKho>();
    }
}
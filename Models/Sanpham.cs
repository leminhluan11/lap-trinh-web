using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Domain.Entities
{
    public class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }

        [Required]
        [StringLength(255)]
        public string TenSanPham { get; set; } = null!;

        [StringLength(255)]
        [Index(IsUnique = true)]
        public string? DuongDan { get; set; }

        [StringLength(255)]
        public string? MoTa { get; set; }

        public int? MaDanhMuc { get; set; }

        [StringLength(255)]
        public string? ThuongHieu { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal GiaGoc { get; set; }

        [StringLength(50)]
        public string? TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        public virtual DanhMuc? DanhMuc { get; set; }
        public virtual ICollection<BienTheSanPham> BienTheSanPhams { get; set; } = new List<BienTheSanPham>();
        public virtual ICollection<DanhGia> DanhGias { get; set; } = new List<DanhGia>();
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Domain.Entities
{
    public class PhieuGiamGia
    {
        [Key]
        public int MaVoucher { get; set; }

        [StringLength(50)]
        [Index(IsUnique = true)]
        public string? MaCode { get; set; }

        [StringLength(50)]
        public string? LoaiGiamGia { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? GiaTriGiam { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? GiaTriDonHangToiThieu { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? GiaTriGiamToiDa { get; set; }

        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        public int? GioiHanSuDung { get; set; }
        public int? SoLanDaDung { get; set; }

        [StringLength(50)]
        public string? TrangThai { get; set; }

        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Domain.Entities
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTietDon { get; set; }

        public int? MaDonHang { get; set; }
        public int? MaBienThe { get; set; }

        public int? SoLuong { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? GiaTaiThoiDiemMua { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TongTienSanPham { get; set; }

        public virtual DonHang? DonHang { get; set; }
        public virtual BienTheSanPham? BienThe { get; set; }
    }
}
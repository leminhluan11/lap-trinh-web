using System;
using System.ComponentModel.DataAnnotations;

namespace YourProjectName.Domain.Entities
{
    public class BaiViet
    {
        [Key]
        public int MaBaiViet { get; set; }

        [StringLength(255)]
        public string? TieuDe { get; set; }

        [StringLength(255)]
        public string? DuongDan { get; set; }

        [StringLength(255)]   // Nên đổi thành nvarchar(max) sau này
        public string? NoiDung { get; set; }

        [StringLength(255)]
        public string? AnhDaiDien { get; set; }

        public int? MaTacGia { get; set; }

        public DateTime? NgayXuatBan { get; set; }

        [StringLength(50)]
        public string? TrangThai { get; set; }

        public virtual NguoiDung? TacGia { get; set; }
    }
}
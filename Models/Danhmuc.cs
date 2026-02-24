using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Domain.Entities
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [StringLength(255)]
        public string? TenDanhMuc { get; set; }

        public int? MaDanhMucCha { get; set; }

        [StringLength(255)]
        [Index(IsUnique = true)]
        public string? DuongDan { get; set; }

        [StringLength(255)]
        public string? MoTa { get; set; }

        public virtual DanhMuc? DanhMucCha { get; set; }
        public virtual ICollection<DanhMuc> DanhMucCons { get; set; } = new List<DanhMuc>();
        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
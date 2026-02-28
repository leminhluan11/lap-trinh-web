using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("Orders")]
public class Order
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string OrderCode { get; set; } = null!;

    public int? UserId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(100)]
    public string ShippingName { get; set; } = null!;

    [Required, MaxLength(500)]
    public string ShippingAddress { get; set; } = null!;

    [Required, MaxLength(15)]
    public string ShippingPhone { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; } = 0;

    [MaxLength(50)]
    public string? CouponCode { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingFee { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalAmount { get; set; }

    [Required, MaxLength(50)]
    public string PaymentMethod { get; set; } = null!;

    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "Unpaid";

    public int Status { get; set; } = 0;

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    public virtual ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    public virtual ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
    public virtual ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
}
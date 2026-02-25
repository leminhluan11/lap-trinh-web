[Table("Orders")]
public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string OrderCode { get; set; } = string.Empty;

    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    [Required]
    [StringLength(100)]
    public string ShippingName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required]
    [StringLength(15)]
    public string ShippingPhone { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountAmount { get; set; }

    [StringLength(50)]
    public string? CouponCode { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ShippingFee { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalAmount { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    [StringLength(20)]
    public string? PaymentStatus { get; set; }

    public int? Status { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();
    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
namespace FashionEcommerce.DTOs;

public class OrderStatisticsDto
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalShippingFee { get; set; }
    public decimal AverageOrderValue { get; set; }
    public List<OrderByStatusDto> OrdersByStatus { get; set; } = new List<OrderByStatusDto>();
    public List<OrderByPaymentStatusDto> OrdersByPaymentStatus { get; set; } = new List<OrderByPaymentStatusDto>();
}

public class OrderByStatusDto
{
    public int Status { get; set; }
    public int Count { get; set; }
    public decimal Amount { get; set; }
}

public class OrderByPaymentStatusDto
{
    public string PaymentStatus { get; set; } = null!;
    public int Count { get; set; }
    public decimal Amount { get; set; }
}
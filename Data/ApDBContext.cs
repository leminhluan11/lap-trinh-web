using Microsoft.EntityFrameworkCore;
using FashionEcommerce.Models;

namespace FashionEcommerce.Data
{
    public class FashionEcommerceDbContext : DbContext
    {
        public FashionEcommerceDbContext(DbContextOptions<FashionEcommerceDbContext> options)
            : base(options)
        {
        }

        // ==================== 20 DbSet (đúng tên bảng trong DB) ====================
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserAddress> UserAddresses { get; set; } = null!;
        public DbSet<ArticleCategory> ArticleCategories { get; set; } = null!;
        public DbSet<Article> Articles { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Coupon> Coupons { get; set; } = null!;
        public DbSet<MasterColor> MasterColors { get; set; } = null!;
        public DbSet<MasterSize> MasterSizes { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<ProductPromotion> ProductPromotions { get; set; } = null!;
        public DbSet<ProductReview> ProductReviews { get; set; } = null!;
        public DbSet<ProductVariant> ProductVariants { get; set; } = null!;
        public DbSet<Promotion> Promotions { get; set; } = null!;
        public DbSet<PromotionCondition> PromotionConditions { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================== UNIQUE CONSTRAINTS (đúng như DB gốc) ====================
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Slug).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Slug).IsUnique();
            modelBuilder.Entity<Article>().HasIndex(a => a.Slug).IsUnique();
            modelBuilder.Entity<ArticleCategory>().HasIndex(ac => ac.Slug).IsUnique();
            modelBuilder.Entity<ProductVariant>().HasIndex(v => v.Sku).IsUnique();
            modelBuilder.Entity<Order>().HasIndex(o => o.OrderCode).IsUnique();
            modelBuilder.Entity<Coupon>().HasIndex(c => c.Code).IsUnique();

            // ==================== CHECK CONSTRAINTS (đúng như DB gốc) ====================
            modelBuilder.Entity<ProductReview>()
                .HasCheckConstraint("CK_ProductReviews_Rating", "[Rating] >= 1 AND [Rating] <= 5");

            modelBuilder.Entity<Promotion>()
                .HasCheckConstraint("CK_Promotions_DiscountType", 
                    "[DiscountType] = 'FIXED_AMOUNT' OR [DiscountType] = 'PERCENTAGE'");

            // ==================== RELATIONSHIPS & DELETE BEHAVIOR (đúng như FK trong script SQL) ====================

            // Cascade Delete
            modelBuilder.Entity<UserAddress>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.Addresses)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductPromotion>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.Promotions)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductPromotion>()
                .HasOne(pp => pp.Promotion)
                .WithMany(p => p.ProductPromotions)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariant>()
                .HasOne(v => v.Product)
                .WithMany(p => p.Variants)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PromotionCondition>()
                .HasOne(pc => pc.Promotion)
                .WithMany(p => p.Conditions)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.Details)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderStatusHistory>()
                .HasOne(h => h.Order)
                .WithMany(o => o.StatusHistory)
                .OnDelete(DeleteBehavior.Cascade);

            // Self-referential Category (Parent - Children)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);   // Không cascade

            // ProductReview - Restrict (không cascade)
            modelBuilder.Entity<ProductReview>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductReview>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductReview>()
                .HasOne(r => r.Order)
                .WithMany(o => o.Reviews)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - User (UserId có thể NULL)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .OnDelete(DeleteBehavior.SetNull);

            // Coupon - User & Promotion (Restrict)
            modelBuilder.Entity<Coupon>()
                .HasOne(c => c.User)
                .WithMany(u => u.Coupons)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Variant)
                .WithMany(v => v.CartItems)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Variant)
                .WithMany(v => v.OrderDetails)
                .OnDelete(DeleteBehavior.Restrict);

            // Article - ArticleCategory
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .OnDelete(DeleteBehavior.Restrict);

            // Product - Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
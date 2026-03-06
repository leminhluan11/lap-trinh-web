using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionEcommerce.Data;
using FashionEcommerce.Models;
using Microsoft.AspNetCore.Authorization;
using FashionEcommerce.DTOs;  // Đảm bảo using này để dùng DTO

namespace FashionEcommerce.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly FashionEcommerceDbContext _context;

        public ProductsController(FashionEcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET /api/admin/products - Danh sách sản phẩm
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSummaryDto>>> GetProducts()
        {
            var products = await _context.Products
.Include(p => p.Category)
.Select(p => new ProductSummaryDto
{
    Id = p.Id,
    Name = p.Name,
    Slug = p.Slug,
    Price = p.Price,
    Description = p.Description,
    Thumbnail = p.Thumbnail,
    IsActive = p.IsActive,
    CategoryId = p.CategoryId,
    CategoryName = p.Category.Name,

    Rating = _context.ProductReviews
        .Where(r => r.ProductId == p.Id)
        .Select(r => (double?)r.Rating)
        .Average() ?? 0,

    ReviewCount = _context.ProductReviews
        .Count(r => r.ProductId == p.Id)
})
.ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// POST /api/admin/products - Tạo mới sản phẩm
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductSummaryDto>> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Slug))
                return BadRequest(new { message = "Name và Slug không được để trống" });

            // Kiểm tra slug đã tồn tại
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Slug == request.Slug);
            if (existingProduct != null)
                return BadRequest(new { message = "Slug đã tồn tại" });

            // Kiểm tra category có tồn tại
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                return BadRequest(new { message = "Danh mục sản phẩm không tồn tại" });

            var product = new Product
            {
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Thumbnail = request.Thumbnail,
                IsActive = request.IsActive ?? true
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Trả về DTO thay vì anonymous
            var response = new ProductSummaryDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Price = product.Price,
                IsActive = product.IsActive
            };

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, response);
        }

        /// <summary>
        /// GET /api/admin/products/{id} - Chi tiết sản phẩm
        /// </summary>
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailDto>> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            var dto = new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                Thumbnail = product.Thumbnail,
                IsActive = product.IsActive,
                Category = new CategoryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                },
                Images = product.Images.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    SortOrder = i.SortOrder
                }).ToList(),
                Variants = product.Variants.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    ColorId = v.ColorId,
                    SizeId = v.SizeId,
                    Sku = v.Sku,
                    Quantity = v.Quantity,
                    PriceModifier = v.PriceModifier
                }).ToList()
            };

            return Ok(dto);
        }

        /// <summary>
        /// PUT /api/admin/products/{id} - Cập nhật sản phẩm
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            // Nếu slug được thay đổi, kiểm tra xem có trùng không
            if (!string.IsNullOrEmpty(request.Slug) && request.Slug != product.Slug)
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Slug == request.Slug && p.Id != id);
                if (existingProduct != null)
                    return BadRequest(new { message = "Slug đã tồn tại" });

                product.Slug = request.Slug;
            }

            // Nếu categoryId được thay đổi, kiểm tra category có tồn tại
            if (request.CategoryId.HasValue && request.CategoryId.Value != product.CategoryId)
            {
                var category = await _context.Categories.FindAsync(request.CategoryId.Value);
                if (category == null)
                    return BadRequest(new { message = "Danh mục sản phẩm không tồn tại" });

                product.CategoryId = request.CategoryId.Value;
            }

            if (!string.IsNullOrEmpty(request.Name))
                product.Name = request.Name;

            if (request.Price.HasValue)
                product.Price = request.Price.Value;

            if (!string.IsNullOrEmpty(request.Description))
                product.Description = request.Description;

            if (!string.IsNullOrEmpty(request.Thumbnail))
                product.Thumbnail = request.Thumbnail;

            if (request.IsActive.HasValue)
                product.IsActive = request.IsActive.Value;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật sản phẩm thành công", product.Id });
        }

        /// <summary>
        /// DELETE /api/admin/products/{id} - Xóa sản phẩm
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa sản phẩm thành công" });
        }
        [HttpGet("featured")]
public async Task<IActionResult> GetFeaturedProduct()
{
    var product = await _context.Products
        .Select(p => new
        {
            p.Id,
            p.Name,
            p.Price,
            p.Thumbnail,
            Rating = _context.ProductReviews
                .Where(r => r.ProductId == p.Id)
                .Average(r => (double?)r.Rating) ?? 0
        })
        .OrderByDescending(p => p.Rating)
        .FirstOrDefaultAsync();

    return Ok(product);
}
    }

    // Giữ nguyên request DTO cũ của bạn
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? Thumbnail { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UpdateProductRequest
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public string? Thumbnail { get; set; }
        public bool? IsActive { get; set; }
    }
}
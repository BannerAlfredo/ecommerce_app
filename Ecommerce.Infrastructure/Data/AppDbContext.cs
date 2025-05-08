using Microsoft.EntityFrameworkCore;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor para uso en tiempo de ejecución (con inyección de dependencias)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Constructor para uso en tiempo de diseño (necesario para migraciones)
        protected AppDbContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Discount> Discounts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.100.24;Database=EcommerceDB;User Id=sa;Password=Password2307;TrustServerCertificate=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.PasswordSalt).IsRequired();
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // Address
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.AddressId);
                entity.Property(a => a.StreetAddress).IsRequired().HasMaxLength(100);
                entity.Property(a => a.City).IsRequired().HasMaxLength(50);
                entity.Property(a => a.State).IsRequired().HasMaxLength(50);
                entity.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
                entity.Property(a => a.Country).IsRequired().HasMaxLength(50);
                entity.Property(a => a.AddressType).IsRequired().HasMaxLength(10);
            });

            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                entity.Property(p => p.ImageUrl).HasMaxLength(255);
                entity.HasIndex(p => p.Name);
            });

            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Description).HasMaxLength(200);
                entity.Property(c => c.ImageUrl).HasMaxLength(255);
                entity.HasIndex(c => c.Name);
            });

            // Cart
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.CartId);
                entity.Property(c => c.SessionId).HasMaxLength(100);
                entity.Property(c => c.TotalAmount).HasColumnType("decimal(18,2)");
            });

            // CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.CartItemId);
                entity.Property(ci => ci.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(ci => ci.Subtotal).HasColumnType("decimal(18,2)");
            });

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.OrderStatus).IsRequired().HasMaxLength(20);
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(o => o.ShippingMethod).HasMaxLength(50);
                entity.Property(o => o.ShippingCost).HasColumnType("decimal(18,2)");
                entity.Property(o => o.OrderNotes).HasMaxLength(500);
            });

            // OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.OrderItemId);
                entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.Subtotal).HasColumnType("decimal(18,2)");
            });

            // Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);
                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                entity.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);
                entity.Property(p => p.PaymentStatus).IsRequired().HasMaxLength(20);
                entity.Property(p => p.TransactionId).HasMaxLength(100);
                entity.Property(p => p.CardLastFour).HasMaxLength(4);
                entity.Property(p => p.CardType).HasMaxLength(20);
            });

            // Discount
            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(d => d.DiscountId);
                entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
                entity.Property(d => d.Description).HasMaxLength(100);
                entity.Property(d => d.DiscountValue).HasColumnType("decimal(18,2)");
                entity.Property(d => d.DiscountType).IsRequired().HasMaxLength(20);
                entity.HasIndex(d => d.Code).IsUnique();
            });

            // Relaciones

            // User - Address
            modelBuilder.Entity<User>()
                .HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            // User - Order
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            // User - Cart
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);

            // Category - Product
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            // Category - Subcategories
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Subcategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId)
                .IsRequired(false);

            // Cart - CartItems
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);

            // Cart - Discounts (muchos a muchos)
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.AppliedDiscounts)
                .WithMany(d => d.AppliedCarts)
                .UsingEntity(j => j.ToTable("CartDiscounts"));

            // Product - CartItem
            modelBuilder.Entity<Product>()
                .HasMany<CartItem>()
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId);

            // Product - OrderItem
            modelBuilder.Entity<Product>()
                .HasMany<OrderItem>()
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId);

            // Order - OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            // Order - Payments
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId);

            // Order - ShippingAddress
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - BillingAddress
            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
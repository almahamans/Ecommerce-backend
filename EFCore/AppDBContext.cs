using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// must download the packages/dependincy needed to use the Dbcontext
public class AppDbContext : DbContext
{

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  { }

  // database set = table
  public DbSet<Product> Products { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<Address> Addresses { get; set; }
  public DbSet<Shipment> Shipments { get; set; }
  public DbSet<OrderProduct> OrderProducts { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Product>(entity =>
   {
     entity.HasKey(p => p.ProductId);
     entity.Property(p => p.ProductId).HasDefaultValueSql("uuid_generate_v4()");
     entity.Property(p => p.ProductName).IsRequired().HasMaxLength(100);
     entity.Property(p => p.Description).IsRequired().HasMaxLength(100);
     entity.Property(p => p.Price).IsRequired();
     entity.Property(p => p.Quantity).IsRequired();
     entity.Property(p => p.Image).IsRequired();
     entity.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
   });
    
    modelBuilder.Entity<Category>(entity =>
    {
      entity.HasKey(category => category.CategoryId);
      entity.Property(c => c.CategoryId).HasDefaultValueSql("uuid_generate_v4()");
      entity.Property(c => c.CategoryName).IsRequired().HasMaxLength(100);
      entity.HasIndex(c => c.CategoryName).IsUnique();
    });

    modelBuilder.Entity<Category>()
    .HasMany(c => c.Products) // many products has one category
    .WithOne(p => p.Category)  //every product belongs to a single category.
    .HasForeignKey(p => p.CategoryId)
    .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(u => u.UserId);
      entity.Property(u => u.UserId).HasDefaultValueSql("uuid_generate_v4()");
      entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
      entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
      entity.HasIndex(u => u.Email).IsUnique();
      entity.Property(u => u.Password).IsRequired();
      entity.Property(u => u.Role);
      entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    modelBuilder.Entity<Address>(attribut =>
    {
      attribut.HasKey(a => a.AddresId);
      attribut.Property(a => a.AddresId).HasDefaultValueSql("uuid_generate_v4()");
      attribut.Property(a => a.City).HasMaxLength(50);
      attribut.Property(a => a.Neighberhood).HasMaxLength(100);
      attribut.Property(a => a.Street).HasMaxLength(500);
    });

    modelBuilder.Entity<User>()
    .HasMany(u => u.Addresses)
    .WithOne(a => a.User)
    .HasForeignKey(a => a.UserId)
    .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Order>(attribut =>
    {
      attribut.HasKey(o => o.OrderId);
      attribut.Property(o => o.OrderId).HasDefaultValueSql("uuid_generate_v4()");
      attribut.Property(o => o.TotalAmount).IsRequired();
      attribut.Property(o => o.OrderDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
      attribut.HasKey(o => o.OrderId);
      attribut.Property(o => o.OrderId).HasDefaultValueSql("uuid_generate_v4()");
      attribut.Property(o => o.TotalAmount).IsRequired();
      attribut.Property(o => o.OrderDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    modelBuilder.Entity<User>()
      .HasMany(u => u.Orders)
      .WithOne(o => o.User)
      .HasForeignKey(o => o.UserId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Shipment>(attribut =>
    {
      attribut.HasKey(s => s.ShipmentId);
      attribut.Property(s => s.ShipmentId).HasDefaultValueSql("uuid_generate_v4()");
      attribut.Property(s => s.ShipmentStatus);
      attribut.Property(s => s.DeliveryDate);
      attribut.Property(s => s.ShipmentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    modelBuilder.Entity<Order>()
    .HasOne(o => o.Shipment)
    .WithOne(s => s.Order)
    .HasForeignKey<Shipment>(s => s.OrderId)
    .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<OrderProduct>(attribut =>
    {
      attribut.Property(op => op.ProductQuantity);
      attribut.Property(op => op.ProductsPrice);
    });

    modelBuilder.Entity<OrderProduct>()
    .HasKey(op => new { op.OrderId, op.ProductId }); 

    modelBuilder.Entity<OrderProduct>()
    .HasOne(op => op.Order)
    .WithMany(o => o.OrderProducts)
    .HasForeignKey(op => op.OrderId)
    .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<OrderProduct>()
    .HasOne(op => op.Product)
    .WithMany(p => p.OrderProducts)
    .HasForeignKey(op => op.ProductId)
    .OnDelete(DeleteBehavior.Cascade);
  }
}






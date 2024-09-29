using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  { }
  public DbSet<User> Users { get; set; }
  public DbSet<Product> Products { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(u => u.UserId);
      entity.Property(u => u.UserId).HasDefaultValueSql("uuid_generate_v4()");
      entity.HasKey(a => a.AddressId);
      entity.Property(a => a.AddressId).HasDefaultValueSql("uuid_generate_v4()");
      entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
      entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
      entity.HasIndex(u => u.Email).IsUnique();
      entity.Property(u => u.Password).IsRequired();
      entity.Property(u => u.Role);
      entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    });

    modelBuilder.Entity<Product>(entity =>
   {
     entity.HasKey(p => p.ProductId);
     entity.Property(p => p.ProductId).HasDefaultValueSql("uuid_generate_v4()");
     entity.Property(p => p.ProductName).IsRequired().HasMaxLength(100);
     entity.Property(p => p.Description).IsRequired().HasMaxLength(100);
     entity.Property(p => p.Price).IsRequired();
     entity.Property(p => p.Quantity).IsRequired();
     entity.Property(p => p.Image).IsRequired();

   });
  }

}

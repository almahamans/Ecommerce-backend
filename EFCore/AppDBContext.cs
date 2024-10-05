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

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {



    // define the constraints and keys
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
  }

}

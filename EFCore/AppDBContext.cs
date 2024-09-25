using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{

    public AppDBContext(DbContextOptions <AppDBContext> options) : base(options)
    {}
    // all the entites
  // Ex  public DbSet<UserDto> Users {get; set;}
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //ex
      //  modelBuilder.Entity<UserDto>(entity =>{ });
    }
}

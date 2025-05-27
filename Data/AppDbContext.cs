using Microsoft.EntityFrameworkCore;
using NawatechApp.Models;

namespace NawatechApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Product>()
                    .HasOne(p => p.Users)
                    .WithMany(u => u.Products)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<ProductCategory>()
                    .HasOne(pc => pc.Users)
                    .WithMany(u => u.ProductCategories)
                    .HasForeignKey(pc => pc.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
    }

   
}

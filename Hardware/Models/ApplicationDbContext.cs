using Hardware.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Product> Products { get; set; } // Correct DbSet name
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Inventory> Inventories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Specify table names
        modelBuilder.Entity<Admin>().ToTable("Admins");
        modelBuilder.Entity<Product>().ToTable("Product"); // Correct table name for Product

        // Seed data for Admin
        modelBuilder.Entity<Admin>().HasData(
            new Admin { Id = 1, Username = "admin", Password = "admin123" }
        );

        // Configure relationships
        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductCategory)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.ProductCategoryId)
            .OnDelete(DeleteBehavior.Restrict); // or .Cascade if you want to delete products when category is deleted

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Sales)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // or Restrict/Delete as per your requirement

        modelBuilder.Entity<Purchase>()
            .HasOne(p => p.Product)
            .WithMany(pr => pr.Purchases)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // or Restrict/Delete as per your requirement

        // Map Inventory entity to the correct table name
        modelBuilder.Entity<Inventory>().ToTable("Inventory"); // Map to "Inventory" table

        modelBuilder.Entity<Inventory>().HasNoKey();
    }

}

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
    public DbSet<Inventory> Inventory { get; set; }  // Add this line


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships

        // Many-to-One relationship between Product and Inventory
        modelBuilder.Entity<Inventory>()
            .HasKey(i => i.ProductId); // Define primary key for Inventory

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // or Cascade as per your requirement

        // One-to-One relationship between Inventory and Purchase
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Purchase)
            .WithOne(p => p.Inventory)
            .HasForeignKey<Inventory>(i => i.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // or Restrict as per your requirement

        // Seed your data, configure other entities as needed

        // Specify table names if necessary
        modelBuilder.Entity<Admin>().ToTable("Admins");
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Inventory>().ToTable("Inventory");

        // Seed data for Admin
        modelBuilder.Entity<Admin>().HasData(
            new Admin { Id = 1, Username = "admin", Password = "admin123" }
        );
    }

        modelBuilder.Entity<Inventory>().HasNoKey();
    }

}

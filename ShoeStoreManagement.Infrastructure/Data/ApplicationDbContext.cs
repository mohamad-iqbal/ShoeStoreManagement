using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStoreManagement.Domain;
using ShoeStoreManagement.Domain.Entities;

namespace ShoeStoreManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Store - Product ( 1 to Many )
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.StoreId, p.Sku })
                .IsUnique();

            // Store - User ( 1 to Many )
            modelBuilder.Entity<User>()
                .HasOne(u => u.Store)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.StoreId)
                .IsRequired(false);

            // Index
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Store - Order ( 1 to Many )
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Store)
                .WithMany(s => s.Orders)
                .HasForeignKey(o =>  o.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product - ProductVariant ( 1 to Many )
            modelBuilder.Entity<ProductVariant>()
                .HasOne(v => v.Product)
                .WithMany(p => p.ProductVariants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductVariat - OrderItem ( 1 to Many )
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.ProductVariant)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(o => o.ProductVariantId);

            // ProductVariant - InventoryTransaction ( 1 to Many )
            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(i => i.ProductVariant)
                .WithMany(p => p.InventoryTransactions)
                .HasForeignKey(i => i.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer - Order ( 1 to Many )
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o =>o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - OrderItem ( 1 to Many )
            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index
            modelBuilder.Entity<Order>()
                .HasIndex(o => new { o.OrderNumber })
                .IsUnique();

            // String Conversion
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
            
            // Order - InventoryTransaction ( 1 to Many )
            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(i => i.Order)
                .WithMany(o => o.InventoryTransactions)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // String Conversion
            modelBuilder.Entity<InventoryTransaction>()
                .Property(i => i.Type)
                .HasConversion<string>();

            // User - InventoryTransaction ( 1 to Many )
            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(i => i.User)
                .WithMany(u => u.InventoryTransactions)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }

    }
}

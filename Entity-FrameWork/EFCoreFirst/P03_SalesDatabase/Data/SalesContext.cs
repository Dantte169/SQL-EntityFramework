namespace P03_SalesDatabase.Data
{
    using P03_SalesDatabase.Data.Models;
    using System;
    using Microsoft.EntityFrameworkCore;
    public class SalesContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer
                    (@"Server=DESKTOP-MHJ8P43\SQLEXPRESS;Database=SalesDatabase;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
                .HasKey(k => k.StoreId);

            modelBuilder.Entity<Store>()
                .Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode();

            modelBuilder.Entity<Store>()
                .HasMany(s => s.Sales)
                .WithOne(s => s.Store)
                .HasForeignKey(k => k.StoreId);


            modelBuilder.Entity<Customer>()
                .HasKey(k => k.CustomerId);

            modelBuilder.Entity<Customer>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder.Entity<Customer>()
                .Property(p => p.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(s => s.Sales)
                .WithOne(s => s.Customer)
                .HasForeignKey(k => k.CustomerId);

            modelBuilder
                 .Entity<Customer>()
                 .Property(p => p.CreditCardNumber);


            modelBuilder.Entity<Product>()
                .HasKey(k => k.ProductId);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder.Entity<Product>()
                .HasMany(s => s.Sales)
                .WithOne(p => p.Product)
                .HasForeignKey(k => k.ProductId);

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Quantity);

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Price);

            modelBuilder.Entity<Sale>()
                .HasKey(k => k.SaleId);

            modelBuilder.Entity<Sale>()
                 .Property(p => p.Date)
                 .HasDefaultValueSql("GETDATE()");

            modelBuilder
                .Entity<Sale>()
                .HasOne(p => p.Product)
                .WithMany(s => s.Sales)
                .HasForeignKey(p => p.ProductId);

            modelBuilder
                .Entity<Sale>()
                .HasOne(p => p.Customer)
                .WithMany(s => s.Sales)
                .HasForeignKey(p => p.CustomerId);

            modelBuilder
                .Entity<Sale>()
                .HasOne(p => p.Store)
                .WithMany(s => s.Sales)
                .HasForeignKey(p => p.StoreId);


        }

    }
}

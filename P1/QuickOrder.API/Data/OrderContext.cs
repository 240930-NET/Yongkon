using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using QuickOrder.API.Model;

namespace QuickOrder.API.Data;

public partial class OrderContext : DbContext
{
    public OrderContext() {}

    public OrderContext(DbContextOptions<OrderContext> options) : base(options) {}

    public virtual DbSet<Order> Orders {get; set;}
    public virtual DbSet<Item> Items {get; set;}
    public virtual DbSet<OrderItem> OrderItems {get; set;}

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<Order>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.Date = DateTime.Now;
            }
        }
        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>()
          .HasKey(oi => new {oi.OrderId, oi.ItemId});

        modelBuilder.Entity<OrderItem>()
          .HasOne(oi => oi.Order)
          .WithMany(o => o.OrderItems)
          .HasForeignKey(oi =>oi.OrderId);

        modelBuilder.Entity<OrderItem>()
          .HasOne(oi => oi.Item)
          .WithMany(i => i.OrderItems)
          .HasForeignKey(oi => oi.ItemId);
          
        modelBuilder.Entity<OrderItem>()
          .ToTable("OrderItems");
    }

}
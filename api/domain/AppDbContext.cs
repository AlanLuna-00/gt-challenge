using Microsoft.EntityFrameworkCore;
using api.features.users.entities;
using api.features.roles.entities;
using api.features.products.entities;
using api.features.orders.entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders{ get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }


}

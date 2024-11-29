namespace api.Data;

using api.features.users.entities;
using api.features.roles.entities;
using api.features.products.entities;
using api.features.orders.entities;
using BCrypt.Net;
using System;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Gerente" },
                new Role { Name = "Empleado" }
            );
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var adminRole = context.Roles.First(r => r.Name == "Admin");
            var gerenteRole = context.Roles.First(r => r.Name == "Gerente");
            var empleadoRole = context.Roles.First(r => r.Name == "Empleado");

            context.Users.AddRange(
                new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.HashPassword("admin123"),
                    RoleId = adminRole.Id
                },
                new User
                {
                    Username = "gerente",
                    Email = "gerente@example.com",
                    PasswordHash = BCrypt.HashPassword("gerente123"),
                    RoleId = gerenteRole.Id
                },
                new User
                {
                    Username = "empleado",
                    Email = "empleado@example.com",
                    PasswordHash = BCrypt.HashPassword("empleado123"),
                    RoleId = empleadoRole.Id
                }
            );
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "Laptop",
                    Description = "Laptop de alta gama",
                    Price = 1500.00m,
                    Stock = 10,
                    IsDeleted = false
                },
                new Product
                {
                    Name = "Smartphone",
                    Description = "Último modelo de smartphone",
                    Price = 800.00m,
                    Stock = 20,
                    IsDeleted = false
                },
                new Product
                {
                    Name = "Monitor",
                    Description = "Monitor 4K de 27 pulgadas",
                    Price = 400.00m,
                    Stock = 15,
                    IsDeleted = false
                }
            );
            context.SaveChanges();
        }

        if (!context.Orders.Any())
        {
            var empleado = context.Users.First(u => u.Username == "empleado");
            var gerente = context.Users.First(u => u.Username == "gerente");
            var laptop = context.Products.First(p => p.Name == "Laptop");
            var smartphone = context.Products.First(p => p.Name == "Smartphone");
            var monitor = context.Products.First(p => p.Name == "Monitor");

            var order1 = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pendiente,
                CreatedByUserId = empleado.Id,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        ProductId = laptop.Id,
                        Quantity = 2
                    },
                    new OrderDetail
                    {
                        ProductId = smartphone.Id,
                        Quantity = 1
                    }
                }
            };

            laptop.Stock -= 2;
            smartphone.Stock -= 1;

            var order2 = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Confirmada,
                CreatedByUserId = gerente.Id,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        ProductId = monitor.Id,
                        Quantity = 3
                    }
                }
            };

            monitor.Stock -= 3;

            context.Orders.AddRange(order1, order2);
            context.SaveChanges();

            context.Products.UpdateRange(laptop, smartphone, monitor);
            context.SaveChanges();
        }
    }
}

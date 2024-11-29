using api.features.orders.entities;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbContext.Orders
            .Include(o => o.CreatedByUser)
            .Include(o => o.OrderDetails) 
                .ThenInclude(od => od.Product) 
            .FirstOrDefaultAsync(o => o.Id == id);
    }


    public IQueryable<Order> GetAll()
    {
        return _dbContext.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .AsQueryable();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddOrderDetailAsync(OrderDetail detail)
    {
        _dbContext.OrderDetails.Add(detail);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveOrderDetailAsync(OrderDetail detail)
    {
        _dbContext.OrderDetails.Remove(detail);
        await _dbContext.SaveChangesAsync();
    }



}

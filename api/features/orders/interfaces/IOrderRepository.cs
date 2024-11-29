using api.features.orders.entities;

public interface IOrderRepository
{
    Task<Order?> GetByIdWithDetailsAsync(int id);
    IQueryable<Order> GetAll();
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task AddOrderDetailAsync(OrderDetail detail);
    Task RemoveOrderDetailAsync(OrderDetail detail);
}

using api.features.orders.DTOs;
using api.Shared.DTOs;

namespace api.features.orders.interfaces;

public interface IOrderService
{
    Task<PaginatedResult<OrderViewDTO>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<OrderViewDTO> GetOrderDetailAsync(int id);
    Task CreateOrderAsync(CreateOrderDTO dto, int userId);
    Task UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDTO dto);
    Task UpdateOrderAsync(int orderId, UpdateOrderDTO dto);

}

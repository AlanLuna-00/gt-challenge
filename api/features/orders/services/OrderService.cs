using api.features.orders.DTOs;
using api.features.orders.entities;
using api.features.orders.interfaces;
using api.features.products.interfaces;
using api.Shared.DTOs;
using api.Shared.Extensions;
using api.Shared.Excepcions;

namespace api.features.orders.services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<PaginatedResult<OrderViewDTO>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _orderRepository.GetAll();

        var result = await query.Select(order => new OrderViewDTO
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            CreatedByUsername = order.CreatedByUser.Username,
            Details = order.OrderDetails.Select(od => new OrderDetailViewDTO
            {
                ProductId = od.ProductId,
                ProductName = od.Product.Name,
                ProductDescription = od.Product.Description,
                ProductPrice = od.Product.Price,
                Quantity = od.Quantity
            }).ToList()
        }).ToPaginatedResultAsync(page, pageSize, cancellationToken);

        if (!result.Items.Any())
        {
            throw new NotFoundException("No orders found.");
        }

        return result;
    }

    public async Task<OrderViewDTO> GetOrderDetailAsync(int id)
    {
        var order = await _orderRepository.GetByIdWithDetailsAsync(id);
        if (order == null)
        {
            throw new NotFoundException("Order not found.");
        }

        return new OrderViewDTO
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            CreatedByUsername = order.CreatedByUser.Username,
            Details = order.OrderDetails.Select(od => new OrderDetailViewDTO
            {
                ProductId = od.ProductId,
                ProductName = od.Product.Name,
                ProductDescription = od.Product.Description,
                ProductPrice = od.Product.Price,
                Quantity = od.Quantity
            }).ToList()
        };
    }

    public async Task CreateOrderAsync(CreateOrderDTO dto, int userId)
    {
        var order = new Order
        {
            CreatedByUserId = userId,
            OrderDetails = dto.OrderDetails.Select(detail => new OrderDetail
            {
                ProductId = detail.ProductId,
                Quantity = detail.Quantity
            }).ToList()
        };

        foreach (var detail in order.OrderDetails)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {detail.ProductId} not found.");
            }

            if (product.Stock < detail.Quantity)
            {
                throw new BadRequestException($"Insufficient stock for product {detail.ProductId}.");
            }

            product.Stock -= detail.Quantity;
            await _productRepository.UpdateAsync(product);
        }

        await _orderRepository.AddAsync(order);
    }

    public async Task UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDTO dto)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), dto.NewStatus))
        {
            throw new BadRequestException("Invalid status value.");
        }

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new NotFoundException("Order not found.");
        }

        if (dto.NewStatus == OrderStatus.Cancelada)
        {
            foreach (var detail in order.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(detail.ProductId);
                if (product != null)
                {
                    product.Stock += detail.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
            }
        }

        order.Status = dto.NewStatus;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task UpdateOrderAsync(int orderId, UpdateOrderDTO dto)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new NotFoundException("Order not found.");
        }

        if (dto.OrderDate.HasValue)
        {
            order.OrderDate = dto.OrderDate.Value;
        }
        if (dto.Status.HasValue)
        {
            order.Status = dto.Status.Value;
        }

        foreach (var updateDetail in dto.UpdateDetails)
        {
            var existingDetail = order.OrderDetails.FirstOrDefault(d => d.ProductId == updateDetail.ProductId);
            if (existingDetail == null)
            {
                throw new BadRequestException($"Order detail with product ID {updateDetail.ProductId} does not exist.");
            }

            var product = await _productRepository.GetByIdAsync(updateDetail.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {updateDetail.ProductId} not found.");
            }

            int stockChange = existingDetail.Quantity - updateDetail.Quantity;
            if (stockChange > 0)
            {
                product.Stock += stockChange;
            }
            else if (product.Stock < Math.Abs(stockChange))
            {
                throw new BadRequestException($"Insufficient stock for product ID {updateDetail.ProductId}.");
            }
            else
            {
                product.Stock -= Math.Abs(stockChange);
            }

            existingDetail.Quantity = updateDetail.Quantity;
            await _productRepository.UpdateAsync(product);
        }

        foreach (var newDetail in dto.NewDetails)
        {
            var product = await _productRepository.GetByIdAsync(newDetail.ProductId);
            if (product == null || product.Stock < newDetail.Quantity)
            {
                throw new BadRequestException($"Insufficient stock for product ID {newDetail.ProductId}.");
            }

            product.Stock -= newDetail.Quantity;
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = newDetail.ProductId,
                Quantity = newDetail.Quantity
            });
            await _productRepository.UpdateAsync(product);
        }

        await _orderRepository.UpdateAsync(order);
    }
}

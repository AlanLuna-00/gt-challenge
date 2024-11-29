using api.features.orders.DTOs;
using api.features.orders.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _orderService.GetPaginatedAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}/detail")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var orderDetail = await _orderService.GetOrderDetailAsync(id);
        return Ok(orderDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        await _orderService.CreateOrderAsync(dto, userId);
        return Ok(new { message = "Order created successfully." });
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Gerente,Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _orderService.UpdateOrderStatusAsync(id, dto);
        return Ok(new { message = "Order status updated successfully." });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Gerente,Admin")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _orderService.UpdateOrderAsync(id, dto);
        return Ok(new { message = "Order updated successfully." });
    }
}

using System.Text.Json.Serialization;
using client.Shared.Enum;

namespace client.Shared.DTOs;

public class OrderViewDTO
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
    public string CreatedByUsername { get; set; }
    public List<OrderDetailViewDTO> Details { get; set; } = new();
}

public class OrderDetailViewDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderDTO
{
    public List<OrderDetailDTO> OrderDetails { get; set; } = new();
}

public class OrderDetailDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class UpdateOrderDTO
{
    public DateTime? OrderDate { get; set; }
    public string Status { get; set; } // PENDIENTE, CONFIRMADA, PAGADA, FINALIZADA, CANCELADA
    public List<UpdateOrderDetailDTO> UpdateDetails { get; set; } = new();
    public List<OrderDetailDTO> NewDetails { get; set; } = new();
}

public class UpdateOrderDetailDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateOrderStatusDTO
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus NewStatus { get; set; } // PENDIENTE, CONFIRMADA, PAGADA, FINALIZADA, CANCELADA
}
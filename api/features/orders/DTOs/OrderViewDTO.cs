namespace api.features.orders.DTOs;

using api.features.orders.entities;
using System.Text.Json.Serialization;

public class OrderDetailViewDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}

public class OrderViewDTO
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
    public string CreatedByUsername { get; set; }
    public IEnumerable<OrderDetailViewDTO> Details { get; set; } = new List<OrderDetailViewDTO>();
}

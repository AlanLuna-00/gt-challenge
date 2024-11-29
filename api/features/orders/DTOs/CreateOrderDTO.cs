namespace api.features.orders.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateOrderDTO
{
    [Required(ErrorMessage = "OrderDetails is required.")]
    [MinLength(1, ErrorMessage = "At least one order detail is required.")]
    public List<OrderDetailDTO> OrderDetails { get; set; } = new();
}


public class OrderDetailDTO
{
    [Required(ErrorMessage = "ProductId is required.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }
}

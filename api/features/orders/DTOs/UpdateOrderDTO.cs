namespace api.features.orders.DTOs;

using api.features.orders.entities;
using System.ComponentModel.DataAnnotations;

public class UpdateOrderDTO
{
    public DateTime? OrderDate { get; set; }
    public OrderStatus? Status { get; set; }

    [Required(ErrorMessage = "UpdateDetails is required.")]
    public List<UpdateOrderDetailDTO> UpdateDetails { get; set; } = new();

    [Required(ErrorMessage = "NewDetails is required.")]
    public List<OrderDetailDTO> NewDetails { get; set; } = new();
}

public class UpdateOrderDetailDTO
{
    [Required(ErrorMessage = "ProductId is required.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be zero or greater.")]
    public int Quantity { get; set; }
}

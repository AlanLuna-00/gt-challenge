namespace api.features.orders.DTOs;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using api.features.orders.entities;

public class UpdateOrderStatusDTO
{
    [Required(ErrorMessage = "NewStatus is required.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus NewStatus { get; set; }
}
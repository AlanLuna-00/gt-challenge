namespace api.features.orders.entities;

using api.features.products.entities;
using api.features.users.entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pendiente;

    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

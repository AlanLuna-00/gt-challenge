using Microsoft.AspNetCore.Components;
using client.Services;
using client.Shared.DTOs;

namespace client.Pages.Orders;

public partial class UpdateOrder : ComponentBase
{
    [Parameter] public int orderId { get; set; }
    private bool loading = false;
    private string? errorMessage = null;
    private string searchQuery = string.Empty;

    private List<ProductDTO> filteredProducts = new();
    private List<OrderDetailDTO> orderDetails = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadOrderDetails();
        await LoadProducts();
    }

    private async Task LoadOrderDetails()
    {
        try
        {
            loading = true;
            var order = await OrderService.GetDetailAsync(orderId);
            orderDetails = order.Details
                .Select(d => new OrderDetailDTO
                {
                    ProductId = d.ProductId,
                    ProductName = d.ProductName,
                    Quantity = d.Quantity,
                    Price = d.ProductPrice,
                })
                .ToList();
        }
        catch (Exception ex)
        {
            errorMessage = $"Error al cargar la orden: {ex.Message}";
        }
        finally
        {
            loading = false;
        }
    }

    private async Task LoadProducts()
    {
        try
        {
            var result = await ProductService.GetAllAsync(1, 50);
            filteredProducts = result.Items;
        }
        catch (Exception ex)
        {
            errorMessage = $"Error al cargar productos: {ex.Message}";
        }
    }

    private async Task SearchProducts()
    {
        try
        {
            loading = true;
            var result = await ProductService.SearchProductsAsync(searchQuery, 1, 50);
            filteredProducts = result.Items;
        }
        finally
        {
            loading = false;
        }
    }

    private void ClearSearch()
    {
        searchQuery = string.Empty;
        _ = LoadProducts();
    }

    private void AddProductToOrder(ProductDTO product)
    {
        if (orderDetails.Any(o => o.ProductId == product.Id)) return;

        orderDetails.Add(new OrderDetailDTO
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = 1,
            Stock = product.Stock
        });
    }

    private void RemoveProductFromOrder(OrderDetailDTO detail)
    {
        orderDetails.Remove(detail);
    }

    private async Task SubmitOrder()
    {
        var updateOrderDTO = new UpdateOrderDTO
        {
            UpdateDetails = orderDetails.Select(d => new UpdateOrderDetailDTO
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity
            }).ToList()
        };

        try
        {
            await OrderService.UpdateAsync(orderId, updateOrderDTO);
            NavigationManager.NavigateTo("/Orders");
        }
        catch (Exception ex)
        {
            errorMessage = $"Error al actualizar la orden: {ex.Message}";
        }
    }
}

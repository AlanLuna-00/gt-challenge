using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Orders;

public partial class CreateOrder : ComponentBase
{
    private bool loading = false;
    private bool isAuthenticated = true;
    private string searchQuery = string.Empty;

    private List<ProductDTO> filteredProducts = new();
    private List<OrderDetailDTO> orderDetails = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadProducts();
    }

    private async Task LoadProducts()
    {
        loading = true;
        try
        {
            var result = await ProductService.GetAllAsync(1, 50); // Cargar productos iniciales
            filteredProducts = result.Items;
        }
        finally
        {
            loading = false;
        }
    }

    private async Task SearchProducts()
    {
        loading = true;
        try
        {
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
        var createOrderDTO = new CreateOrderDTO
        {
            OrderDetails = orderDetails.Select(o => new OrderDetailDTO
            {
                ProductId = o.ProductId,
                Quantity = o.Quantity
            }).ToList()
        };

        try
        {
            await OrderService.CreateAsync(createOrderDTO);
            NavigationManager.NavigateTo("/Orders");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear la orden: {ex.Message}");
        }
    }
}
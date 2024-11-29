using client.Shared.DTOs;
using client.Shared.Enum;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Orders;

public partial class Order : ComponentBase
{
    private List<OrderViewDTO> orders = new();
    private string? errorMessage;
    private bool loading;

    private int currentPage = 1;
    private int pageSize = 10;
    private int totalPages = 1;
    private bool hasNextPage = false;
    private bool hasPreviousPage = false;
    private bool canManageOrders = false;

    protected override async Task OnInitializedAsync()
    {
        await UpdateUserPermissions();
        await LoadOrders();
    }

    private async Task UpdateUserPermissions()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        canManageOrders = user.IsInRole("Admin") || user.IsInRole("Gerente");
    }

    private async Task LoadOrders()
    {
        try
        {
            loading = true;
            var paginatedResult = await OrderService.GetPaginatedAsync(currentPage, pageSize);
            orders = paginatedResult.Items;
            totalPages = (int)Math.Ceiling((double)paginatedResult.TotalCount / pageSize);
            hasNextPage = currentPage < totalPages;
            hasPreviousPage = currentPage > 1;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        finally
        {
            loading = false;
        }
    }

    private void ViewDetails(int id)
    {
        NavigationManager.NavigateTo($"/Orders/{id}/detail");
    }

    private void CreateOrder()
    {
        NavigationManager.NavigateTo("/Orders/create");
    }

    private void UpdateOrder(int id)
    {
        NavigationManager.NavigateTo($"/Orders/edit/{id}");
    }
    
    private async Task ChangeOrderStatus(int orderId, ChangeEventArgs e)
    {
        var newStatus = Enum.Parse<OrderStatus>(e.Value?.ToString() ?? string.Empty);

        try
        {
            var updateStatusDto = new UpdateOrderStatusDTO
            {
                NewStatus = newStatus
            };

            await OrderService.UpdateStatusAsync(orderId, updateStatusDto);
            await LoadOrders();
        }
        catch (Exception ex)
        {
            errorMessage = $"Error al cambiar el estado de la orden: {ex.Message}";
        }
    }

    private void UpdateStatus(int id)
    {
        NavigationManager.NavigateTo($"/Orders/{id}/status");
    }

    private async Task PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            await LoadOrders();
        }
    }

    private async Task NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
            await LoadOrders();
        }
    }
}
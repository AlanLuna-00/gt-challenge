using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Orders;

public partial class ViewOrder : ComponentBase
{
    [Parameter]
    public int OrderId { get; set; }

    private OrderViewDTO? order;
    private string? errorMessage;
    private bool loading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadOrderDetails();
    }

    private async Task LoadOrderDetails()
    {
        try
        {
            order = await OrderService.GetDetailAsync(OrderId);
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

    private void GoBack()
    {
        NavigationManager.NavigateTo("/Orders");
    }
}
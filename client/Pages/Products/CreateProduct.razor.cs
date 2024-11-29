using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Products;

public partial class CreateProduct : ComponentBase
{
    private CreateProductDTO product = new();
    private string? errorMessage;

    private async Task HandleSubmit()
    {
        try
        {
            await ProductService.CreateAsync(product);
            NavigationManager.NavigateTo("/Products");
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
    }

    private void Cancel()
    {
        NavigationManager.NavigateTo("/Products");
    }
}
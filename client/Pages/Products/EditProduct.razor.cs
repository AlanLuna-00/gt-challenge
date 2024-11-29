using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Products;

public partial class EditProduct : ComponentBase
{
    [Parameter] public int Id { get; set; }
    private UpdateProductDTO product = new();
    private string? errorMessage;
    private bool loading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var existingProduct = await ProductService.GetByIdAsync(Id);
            product = new UpdateProductDTO
            {
                Name = existingProduct.Name,
                Description = existingProduct.Description,
                Price = existingProduct.Price,
                Stock = existingProduct.Stock
            };
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

    private async Task HandleSubmit()
    {
        try
        {
            await ProductService.UpdateAsync(Id, product);
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
using System.Text.Encodings.Web;
using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace client.Pages.Products;

public partial class Products : ComponentBase
{
    private List<ProductDTO> products = new();
    private string? errorMessage;
    private bool loading;

    private int currentPage = 1;
    private int pageSize = 10;
    private int totalPages = 1;
    private bool hasNextPage = false;
    private bool hasPreviousPage = false;
    private bool canManageProducts = false;
    private bool isAuthenticated = false;

    private string searchQuery = string.Empty;
    private bool includeDeleted = false;

    protected override async Task OnInitializedAsync()
    {
        await CheckAuthentication();
        if (isAuthenticated)
        {
            await UpdateUserPermissions();
            await LoadProducts();
        }
    }

    private async Task UpdateUserPermissions()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        canManageProducts = user.IsInRole("Admin") || user.IsInRole("Gerente");
    }

    private async Task LoadProducts()
    {
        try
        {
            loading = true;

            PaginatedResult<ProductDTO>? paginatedResult;

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                paginatedResult = await ProductService.GetAllAsync(currentPage, pageSize, includeDeleted);
            }
            else
            {
                paginatedResult = await ProductService.SearchProductsAsync(searchQuery, currentPage, pageSize, includeDeleted);
            }

            products = paginatedResult.Items;
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

    private async Task CheckAuthentication()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        isAuthenticated = user.Identity?.IsAuthenticated == true;
    }

    private void CreateProduct()
    {
        NavigationManager.NavigateTo("/Products/create");
    }

    private void EditProduct(int id)
    {
        NavigationManager.NavigateTo($"/Products/edit/{id}");
    }

    private async Task DeleteProduct(int id)
    {
        if (await ConfirmDelete())
        {
            try
            {
                await ProductService.DeleteAsync(id);
                await LoadProducts();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
    
    private Task RestoreProduct()
    {
        return Task.CompletedTask;
    }
    
    private Task<bool> ConfirmDelete()
    {
        return Task.FromResult(true);
    }

    private async Task PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            await LoadProducts();
        }
    }

    private async Task NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
            await LoadProducts();
        }
    }

    private async Task SearchProducts()
    {
        currentPage = 1;
        await LoadProducts();
    }

    private async Task ClearSearch()
    {
        searchQuery = string.Empty;
        currentPage = 1;
        await LoadProducts();
    }
}

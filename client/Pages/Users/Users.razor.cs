using client.Services;
using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Users;

public partial class Users : ComponentBase
{
    private List<UserDTO> users = new();
    private string? errorMessage;
    private bool loading;
    private bool isAuthenticated = false;
    private bool isAuthorized = false;

    private int currentPage = 1;
    private int pageSize = 10;
    private int totalPages = 1;
    private bool hasNextPage = false;
    private bool hasPreviousPage = false;
    private bool canCreateUser = false;

    private string searchQuery = string.Empty;
    private int userToDelete;

    protected override async Task OnInitializedAsync()
    {
        await CheckAuthenticationAndAuthorization();
        if (isAuthorized)
        {
            await LoadUsers();
        }
    }

    private async Task CheckAuthenticationAndAuthorization()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        isAuthenticated = user.Identity?.IsAuthenticated == true;
        isAuthorized = isAuthenticated &&
                       (await RoleService.IsInRoleAsync("Admin") || await RoleService.IsInRoleAsync("Gerente"));

        canCreateUser = user.IsInRole("Admin");
    }

    private async Task LoadUsers()
    {
        try
        {
            loading = true;

            PaginatedResult<UserDTO>? paginatedResult;
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                paginatedResult = await UserService.GetAllAsync(currentPage, pageSize);
            }
            else
            {
                paginatedResult = await UserService.SearchUsersAsync(searchQuery, currentPage, pageSize);
            }

            users = paginatedResult.Items;
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

    private async Task SearchUsers()
    {
        currentPage = 1;
        await LoadUsers();
    }

    private async Task ClearSearch()
    {
        searchQuery = string.Empty;
        currentPage = 1;
        await LoadUsers();
    }

    private void CreateUser()
    {
        NavigationManager.NavigateTo("/users/create");
    }

    private void EditUser(int id)
    {
        NavigationManager.NavigateTo($"/users/edit/{id}");
    }

    private async Task PromptDeleteUser(int id)
    {
        userToDelete = id;
        if (await ConfirmDelete())
        {
            await DeleteUser();
        }
    }

    private async Task DeleteUser()
    {
        try
        {
            await UserService.DeleteAsync(userToDelete);
            await LoadUsers();
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
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
            await LoadUsers();
        }
    }

    private async Task NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
            await LoadUsers();
        }
    }
}

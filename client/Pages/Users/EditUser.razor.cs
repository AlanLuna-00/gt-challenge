using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Users;

public partial class EditUser : ComponentBase
{
    [Parameter] public int Id { get; set; }

    private UpdateUserDTO editUser = new();
    private string? errorMessage;
    private bool loading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadUser();
    }

    private async Task LoadUser()
    {
        try
        {
            var user = await UserService.GetByIdAsync(Id);
            editUser = new UpdateUserDTO
            {
                Username = user.Username,
                Email = user.Email,
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

    private async Task HandleValidSubmit()
    {
        try
        {
            await UserService.UpdateAsync(Id, editUser);
            NavigationManager.NavigateTo("/Users");
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
    }

    private void Cancel()
    {
        NavigationManager.NavigateTo("/Users");
    }
}
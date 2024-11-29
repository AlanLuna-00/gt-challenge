using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Users;

public partial class ChangePassword : ComponentBase
{
    private ChangePasswordDTO passwordModel = new();
    private string confirmPassword = string.Empty;
    private string? errorMessage;
    private bool isAuthenticated = false;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        isAuthenticated = authState.User.Identity?.IsAuthenticated == true;
    }

    private async Task HandleValidSubmit()
    {
        errorMessage = null;

        if (passwordModel.NewPassword != confirmPassword)
        {
            errorMessage = "La nueva contraseña y su confirmación no coinciden.";
            return;
        }

        try
        {
            await UserService.ChangePasswordAsync(passwordModel);
            NavigationManager.NavigateTo("/");
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
    }
}
using Microsoft.AspNetCore.Components;

namespace client.Pages.Auth;

public partial class Login : ComponentBase
{
    private string username = string.Empty;
    private string password = string.Empty;
    private string errorMessage = string.Empty;
    private bool isLoading = false;

    private async Task HandleLogin()
    {
        errorMessage = string.Empty;
        isLoading = true;

        try
        {
            var success = await AuthService.Login(username, password);
            if (success)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                errorMessage = "Invalid username or password.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }
}
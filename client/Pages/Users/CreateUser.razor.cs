using client.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace client.Pages.Users;

public partial class CreateUser : ComponentBase
{
    private CreateUserDTO newUser = new();
    private List<RoleDTO> roles = new();
    private string? errorMessage;
    private bool loading = true;
    private string? selectedRoleId;


    protected override async Task OnInitializedAsync()
    {
        await LoadRoles();
        loading = false;
    }

    private async Task LoadRoles()
    {
        try
        {
            var result = await RoleService.GetAllAsync(1, 100);
            if (result.Items.Count == 0)
            {
                errorMessage = "No se pudo cargar la lista de roles.";
                return;
            }
            roles = result.Items;
        }
        catch (Exception ex)
        {
            errorMessage = $"Error al cargar los roles: {ex.Message}";
        }
    }
    
    private void HandleRoleChange(ChangeEventArgs e)
    {
        selectedRoleId = e.Value?.ToString(); // Asignar el valor seleccionado
        Console.WriteLine($"Rol seleccionado: {selectedRoleId}");
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            if (int.TryParse(selectedRoleId, out var roleId))
            {
                newUser.RoleId = roleId;
            }
            else
            {
                errorMessage = "No se ha seleccionado un rol";
            }
            await UserService.CreateAsync(newUser);
            NavigationManager.NavigateTo("/Users");
        }
        catch (Exception ex)
        {
            errorMessage = $"Error al crear el usuario: {ex.Message}";
        }
    }

    private void Cancel()
    {
        NavigationManager.NavigateTo("/Users");
    }
}
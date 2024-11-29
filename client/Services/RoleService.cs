using System.Net.Http.Json;
using System.Security.Claims;
using client.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace client.Services
{
    public class RoleService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        
        private readonly HttpClient _httpClient;

        public RoleService(AuthenticationStateProvider authStateProvider, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }
        
        public async Task<PaginatedResult<RoleDTO>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"api/Role?page={page}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PaginatedResult<RoleDTO>>();
            }
            else
            {
                throw new Exception("Error al obtener los roles.");
            }
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return user.Identity?.IsAuthenticated == true && user.IsInRole(role);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            return user.Identity?.IsAuthenticated ?? false;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsInRole(this ClaimsPrincipal user, string role)
        {
            return user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role);
        }
    }
}
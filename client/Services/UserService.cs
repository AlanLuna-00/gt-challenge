using System.Net.Http.Json;
using System.Text.Json;
using client.Services;
using client.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace client.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<UserDTO>> GetAllAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"api/User?page={page}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PaginatedResult<UserDTO>>();
            }
            else
            {
                throw new Exception("Error al obtener los usuarios.");
            }
        }
        
        public async Task<PaginatedResult<UserDTO>> SearchUsersAsync(string query, int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedResult<UserDTO>>($"api/User/search?query={query}&page={page}&pageSize={pageSize}");
            return response ?? new PaginatedResult<UserDTO>();
        }
        
        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/User/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDTO>();
            }
            else
            {
                throw new Exception("Usuario no encontrado.");
            }
        }


        public async Task CreateAsync(CreateUserDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error?.Errors.FirstOrDefault()?.Message ?? "Error al crear el usuario.");
            }
        }

        public async Task UpdateAsync(int id, UpdateUserDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/User/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error?.Errors.FirstOrDefault()?.Message ?? "Error al actualizar el usuario.");
            }
        }
        
        public async Task ChangePasswordAsync(ChangePasswordDTO dto)
        {
            var response = await _httpClient.PutAsJsonAsync("api/User/change-password", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error?.Errors.FirstOrDefault()?.Message ?? "Error al cambiar la contraseña.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/User/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(error?.Errors.FirstOrDefault()?.Message ?? "Error al eliminar el usuario.");
            }
        }
    }
}

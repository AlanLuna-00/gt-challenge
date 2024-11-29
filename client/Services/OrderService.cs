using System.Net.Http.Json;
using client.Shared.DTOs;

namespace client.Services;

public class OrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedResult<OrderViewDTO>> GetPaginatedAsync(int page, int pageSize)
    {
        var response = await _httpClient.GetFromJsonAsync<PaginatedResult<OrderViewDTO>>($"api/Order?page={page}&pageSize={pageSize}");
        if (response == null) throw new Exception("No se pudo cargar la lista de órdenes.");
        return response;
    }

    public async Task<OrderViewDTO> GetDetailAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<OrderViewDTO>($"api/Order/{id}/detail");
        if (response == null) throw new Exception("Orden no encontrada.");
        return response;
    }

    public async Task CreateAsync(CreateOrderDTO order)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Order", order);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(int id, UpdateOrderDTO order)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Order/{id}", order);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateStatusAsync(int id, UpdateOrderStatusDTO status)
    {
        var response = await _httpClient.PatchAsJsonAsync($"api/Order/{id}/status", status);
        response.EnsureSuccessStatusCode();
    }
}
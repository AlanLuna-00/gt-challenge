using System.Net.Http.Json;
using client.Shared.DTOs;

namespace client.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedResult<ProductDTO>> GetAllAsync(int page, int pageSize, bool includeDeleted = false)
    {
        var response = await _httpClient.GetFromJsonAsync<PaginatedResult<ProductDTO>>($"api/Product?page={page}&pageSize={pageSize}&includeDeleted={includeDeleted}");
        if (response == null) throw new Exception("No se pudo cargar la lista de productos.");
        return response;
    }
    
    public async Task<PaginatedResult<ProductDTO>> SearchProductsAsync(string query, int page = 1, int pageSize = 10, bool includeDeleted = false)
    {
        var response = await _httpClient.GetFromJsonAsync<PaginatedResult<ProductDTO>>($"api/Product/search?query={query}&page={page}&pageSize={pageSize}&includeDeleted={includeDeleted}");
        return response ?? new PaginatedResult<ProductDTO>();
    }


    public async Task<ProductDTO> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<ProductDTO>($"api/Product/{id}");
        if (response == null) throw new Exception("Producto no encontrado.");
        return response;
    }

    public async Task CreateAsync(CreateProductDTO product)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Product", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(int id, UpdateProductDTO product)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Product/{id}", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Product/{id}");
        response.EnsureSuccessStatusCode();
    }
}
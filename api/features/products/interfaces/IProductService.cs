namespace api.features.products.interfaces;

using api.features.products.entities;
using api.Shared.DTOs;

public interface IProductService
{
    Task<PaginatedResult<Product>> GetAllPaginatedAsync(int page, int pageSize,bool includeDeleted, CancellationToken cancellationToken);
    Task<PaginatedResult<Product>> SearchProductsAsync(string query, int page, int pageSize,bool includeDeleted, CancellationToken cancellationToken);
    Task<Product> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

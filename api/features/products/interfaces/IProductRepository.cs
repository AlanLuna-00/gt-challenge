namespace api.features.products.interfaces;

using api.features.products.entities;

public interface IProductRepository
{
    IQueryable<Product> GetAll();
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

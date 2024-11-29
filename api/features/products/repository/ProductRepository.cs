namespace api.features.products.repositories;

using Microsoft.EntityFrameworkCore;
using api.features.products.entities;
using api.features.products.interfaces;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Product> GetAll()
    {
        return _dbContext.Products.AsQueryable();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product != null)
        {
            product.IsDeleted = true;
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}

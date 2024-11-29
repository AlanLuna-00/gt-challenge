namespace api.features.products.services;

using api.features.products.interfaces;
using api.features.products.entities;
using api.Shared.DTOs;
using api.Shared.Extensions;
using api.Shared.Excepcions;
using Microsoft.EntityFrameworkCore;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PaginatedResult<Product>> GetAllPaginatedAsync(int page, int pageSize,bool includeDeleted, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetAll();
        
        if (!includeDeleted)
        {
            query = query.Where(p => !p.IsDeleted);
        }
        
        var paginatedProducts = await query.ToPaginatedResultAsync(page, pageSize, cancellationToken);

        if (!paginatedProducts.Items.Any())
        {
            throw new NotFoundException("No products found.");
        }

        return paginatedProducts;
    }

    public async Task<PaginatedResult<Product>> SearchProductsAsync(string query, int page, int pageSize, bool includeDeleted, CancellationToken cancellationToken)
    {
        var productsQuery = _productRepository.GetAll();
        
        if (!includeDeleted)
        {
            productsQuery = productsQuery.Where(p => !p.IsDeleted);
        }
            
        productsQuery = productsQuery.Where(p => EF.Functions.Like(p.Name, $"%{query}%") || EF.Functions.Like(p.Description, $"%{query}%"));

        var totalCount = await productsQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            throw new NotFoundException("No products match the search criteria.");
        }

        var products = await productsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<Product>(products, totalCount, page, pageSize);
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException("Product not found.");
        }

        return product;
    }

    public async Task AddAsync(Product product)
    {
        var existingProduct = await _productRepository.GetAll()
            .AnyAsync(p => p.Name == product.Name);

        if (existingProduct)
        {
            throw new ConflictException("A product with the same name already exists.");
        }

        await _productRepository.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        var existingProduct = await GetByIdAsync(product.Id);

        if (existingProduct == null)
        {
            throw new NotFoundException("Product not found.");
        }

        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException("Product not found.");
        }

        await _productRepository.DeleteAsync(id);
    }
}

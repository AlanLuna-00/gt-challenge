namespace api.features.roles.repositories;

using Microsoft.EntityFrameworkCore;
using api.features.roles.entities;
using api.features.roles.interfaces;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _dbContext;

    public RoleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Role> GetAllAsync()
    {
        return _dbContext.Roles.AsQueryable();
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Role role)
    {
        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _dbContext.Roles.FindAsync(id);
        if (role != null)
        {
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();
        }
    }
}

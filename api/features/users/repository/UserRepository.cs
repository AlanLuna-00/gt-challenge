namespace api.features.users.repositories;

using Microsoft.EntityFrameworkCore;
using api.features.users.entities;
using api.features.users.interfaces;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<User> GetAllAsync()
    {
        return _dbContext.Users.Include(u => u.Role).AsQueryable();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateUserPasswordAsync(User user, string password)
    {
        user.PasswordHash = password;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            user.IsDeleted = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }

}

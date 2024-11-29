namespace api.features.users.interfaces;

using api.features.users.entities;

public interface IUserRepository
{
    IQueryable<User> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task UpdateUserAsync(User user);
    Task UpdateUserPasswordAsync(User user, string password);
    Task DeleteAsync(int id);
}

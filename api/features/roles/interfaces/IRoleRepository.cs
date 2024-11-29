namespace api.features.roles.interfaces;

using api.features.roles.entities;

public interface IRoleRepository
{
    IQueryable<Role> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task AddAsync(Role role);
    Task DeleteAsync(int id);
}

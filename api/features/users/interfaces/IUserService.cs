namespace api.features.users.interfaces;

using api.Shared.DTOs;
using api.features.users.DTOs;

public interface IUserService
{
    Task<PaginatedResult<UserDTO>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<PaginatedResult<UserDTO>> SearchUsersAsync(string query, int page, int pageSize, CancellationToken cancellationToken);
    Task<UserDTO> GetByIdAsync(int id);
    Task CreateAsync(CreateUserDTO dto);
    Task UpdateUserAsync(int id, UpdateUserDTO dto);
    Task ChangePassword(int id, ChangePasswordDTO dto);
    Task DeleteAsync(int id);
}
namespace api.features.roles.interfaces;

using api.features.roles.DTOs;
using api.Shared.DTOs;

public interface IRoleService
{
    Task<PaginatedResult<RoleDTO>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task CreateAsync(CreateRoleDTO dto);
}

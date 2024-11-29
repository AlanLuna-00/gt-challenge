using Microsoft.EntityFrameworkCore;

namespace api.features.roles.services;

using api.Shared.DTOs;
using api.Shared.Extensions;
using api.features.roles.DTOs;
using api.features.roles.entities;
using api.features.roles.interfaces;
using api.Shared.Excepcions;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<PaginatedResult<RoleDTO>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var rolesQuery = _roleRepository.GetAllAsync();
        var paginatedRoles = await rolesQuery.ToPaginatedResultAsync(page, pageSize, cancellationToken);

        if (!paginatedRoles.Items.Any())
        {
            throw new NotFoundException("No roles found.");
        }

        return new PaginatedResult<RoleDTO>(
            paginatedRoles.Items.Select(role => new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            }),
            paginatedRoles.TotalCount,
            page,
            pageSize
        );
    }

    public async Task CreateAsync(CreateRoleDTO dto)
    {
        var existingRole = await _roleRepository.GetAllAsync()
            .AnyAsync(r => r.Name == dto.Name);

        if (existingRole)
        {
            throw new ConflictException($"Role with name '{dto.Name}' already exists.");
        }

        var role = new Role
        {
            Name = dto.Name
        };

        await _roleRepository.AddAsync(role);
    }
}
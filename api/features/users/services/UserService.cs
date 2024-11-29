using api.features.users.DTOs;
using api.features.users.entities;
using api.features.users.interfaces;
using api.Shared.DTOs;
using api.Shared.Excepcions;
using Microsoft.EntityFrameworkCore;
using api.Shared.Extensions;

namespace api.features.users.services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResult<UserDTO>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            var usersQuery = _userRepository.GetAllAsync();
            var paginatedUsers = await usersQuery.ToPaginatedResultAsync(page, pageSize, cancellationToken);

            return new PaginatedResult<UserDTO>(
                paginatedUsers.Items.Select(user => new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role.Name.ToString()
                }),
                paginatedUsers.TotalCount,
                page,
                pageSize
            );
        }
        catch (Exception)
        {
            throw new AppException("Error while retrieving users.", 500);
        }
    }

    public async Task<PaginatedResult<UserDTO>> SearchUsersAsync(string query, int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            var usersQuery = _userRepository
                .GetAllAsync()
                .Where(u => EF.Functions.Like(u.Username, $"%{query}%") || EF.Functions.Like(u.Email, $"%{query}%"));

            var totalCount = await usersQuery.CountAsync(cancellationToken);

            var users = await usersQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role.Name.ToString()
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResult<UserDTO>(users, totalCount, page, pageSize);
        }
        catch (Exception)
        {
            throw new AppException("Error while searching users.", 500);
        }
    }

    public async Task<UserDTO> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found.");
        }

        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.Name,
            IsDeleted = user.IsDeleted
        };
    }

    public async Task CreateAsync(CreateUserDTO dto)
    {
        try
        {
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = dto.RoleId
            };

            await _userRepository.AddAsync(user);
        }
        catch (DbUpdateException)
        {
            throw new ConflictException("A user with the same email or username already exists.");
        }
        catch (Exception)
        {
            throw new AppException("Error while creating user.", 500);
        }
    }

    public async Task UpdateUserAsync(int id, UpdateUserDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null || user.IsDeleted)
        {
            throw new NotFoundException($"User with ID {id} not found.");
        }

        if (!string.IsNullOrEmpty(dto.Username)) user.Username = dto.Username;
        if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.NewPassword)) user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        try
        {
            await _userRepository.UpdateUserAsync(user);
        }
        catch (Exception)
        {
            throw new AppException("Error while updating user.", 500);
        }
    }

    public async Task ChangePassword(int id, ChangePasswordDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null || user.IsDeleted)
        {
            throw new NotFoundException($"User with ID {id} not found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
        {
            throw new ValidationException("The current password is incorrect.");
        }

        var newPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        try
        {
            await _userRepository.UpdateUserPasswordAsync(user, newPassword);
        }
        catch (Exception)
        {
            throw new AppException("Error while changing the password.", 500);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null || user.IsDeleted)
        {
            throw new NotFoundException($"User with ID {id} not found.");
        }

        try
        {
            await _userRepository.DeleteAsync(id);
        }
        catch (Exception)
        {
            throw new AppException("Error while deleting user.", 500);
        }
    }
}

namespace api.features.users.controllers;

using Microsoft.AspNetCore.Mvc;
using api.features.users.DTOs;
using api.features.users.interfaces;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize(Roles = "Admin, Gerente")]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var paginatedResult = await _userService.GetAllPaginatedAsync(page, pageSize, cancellationToken);
        return Ok(paginatedResult);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _userService.SearchUsersAsync(query, page, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid data provided.", details = ModelState });
        }

        await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), null, new { message = "User created successfully." });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid data provided.", details = ModelState });
        }

        await _userService.UpdateUserAsync(id, dto);
        return Ok(new { message = "User updated successfully." });
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid data provided.", details = ModelState });
        }
        var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        await _userService.ChangePassword(userId, dto);
        return Ok(new { message = "Password changed successfully." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteAsync(id);
        return Ok(new { message = "User deleted successfully." });
    }
}

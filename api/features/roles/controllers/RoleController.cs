namespace api.features.roles.controllers;

using Microsoft.AspNetCore.Mvc;
using api.features.roles.DTOs;
using api.features.roles.interfaces;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var paginatedResult = await _roleService.GetAllPaginatedAsync(page, pageSize, cancellationToken);
        return Ok(paginatedResult);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Gerente")]
    public async Task<IActionResult> Create([FromBody] CreateRoleDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid data provided.", details = ModelState });
        }

        await _roleService.CreateAsync(dto);
        return Ok(new { message = "Role created successfully." });
    }
}
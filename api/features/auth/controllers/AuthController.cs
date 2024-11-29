namespace api.features.auth.controllers;

using Microsoft.AspNetCore.Mvc;
using api.features.auth.interfaces;
using api.features.users.interfaces;
using api.features.auth.DTOs;
using api.features.auth.services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public AuthController(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = _userRepository.GetAllAsync()
            .FirstOrDefault(u => u.Username == dto.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Credenciales incorrectas" });
        }

        var token = _authService.GenerateJwtToken(user);
        return Ok(new { token });
    }
}

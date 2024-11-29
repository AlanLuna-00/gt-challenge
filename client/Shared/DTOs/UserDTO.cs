namespace client.Shared.DTOs;


public class UserDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}

public class CreateUserDTO
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
}

public class UpdateUserDTO
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
    public int RoleId { get; set; }
}

public class ChangePasswordDTO
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
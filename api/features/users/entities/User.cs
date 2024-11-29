﻿namespace api.features.users.entities;

using api.features.roles.entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public bool IsDeleted { get; set; }
}
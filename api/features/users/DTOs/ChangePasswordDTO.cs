using System.ComponentModel.DataAnnotations;

namespace api.features.users.DTOs;

public class ChangePasswordDTO
{
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [Required]
    public string CurrentPassword { get; set; }
    
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [Required]
    public string NewPassword { get; set; }
}   